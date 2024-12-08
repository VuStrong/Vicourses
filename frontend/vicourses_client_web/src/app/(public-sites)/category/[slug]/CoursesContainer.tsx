"use client";

import { useEffect, useState } from "react";
import { IoFilterSharp } from "react-icons/io5";
import { Select, Option, Drawer, Typography } from "@material-tailwind/react";
import { Category } from "@/libs/types/category";
import { Course, CourseLevel } from "@/libs/types/course";
import { getCourses } from "@/services/api/course";
import { Loader } from "@/components/common";
import CoursesGrid from "@/components/course/CoursesGrid";
import CourseLevelFilter from "@/components/filters/CourseLevelFilter";
import CoursePriceFilter from "@/components/filters/CoursePriceFilter";
import CourseRatingFilter from "@/components/filters/CourseRatingFilter";

type CourseFilterOptions = {
    free?: boolean;
    level?: CourseLevel;
    rating?: number;
};
const limit = 16;

export default function CoursesContainer({ category }: { category: Category }) {
    const isRootCategory = category.parentId === null;

    const [isFilterPanelOpen, setIsFilterPanelOpen] = useState<boolean>(false);
    const [filter, setFilter] = useState<CourseFilterOptions>({});
    const [sort, setSort] = useState<string>("Newest");

    const [isLoadingCourses, setIsLoadingCourses] = useState<boolean>(false);
    const [courses, setCourses] = useState<Course[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [totalCourses, setTotalCourses] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(false);

    const getMoreCourses = async () => {
        const data = await getCourses({
            skip: skip + limit,
            limit,
            ...filter,
            sort: sort as any,
            categoryId: isRootCategory ? category.id : undefined,
            subCategoryId: isRootCategory ? undefined : category.id,
        });

        if (data) {
            setCourses([...courses, ...data.items]);
            setSkip(skip + limit);
            setEnd(data.end);
        }
    };

    useEffect(() => {
        (async () => {
            setIsLoadingCourses(true);

            const data = await getCourses({
                skip: 0,
                limit,
                ...filter,
                sort: sort as any,
                categoryId: isRootCategory ? category.id : undefined,
                subCategoryId: isRootCategory ? undefined : category.id,
            });

            if (data) {
                setTotalCourses(data.total);
                setSkip(0);
                setEnd(data.end);
                setCourses(data.items);
                setIsLoadingCourses(false);
            }
        })();
    }, [filter, sort]);

    return (
        <>
            <Drawer
                open={isFilterPanelOpen}
                onClose={() => setIsFilterPanelOpen(false)}
                className="p-4 overflow-y-auto"
                overlayProps={{
                    className: "fixed",
                }}
            >
                <div className="mb-6 flex items-center justify-between">
                    <Typography variant="h5" color="blue-gray">
                        Filter
                    </Typography>
                    <div>{totalCourses} results</div>
                </div>
                <div>
                    <CourseRatingFilter
                        disabled={isLoadingCourses}
                        rating={filter.rating}
                        onRatingChange={(rating) => {
                            setFilter({
                                ...filter,
                                rating,
                            });
                        }}
                    />
                    <CoursePriceFilter
                        disabled={isLoadingCourses}
                        free={filter.free}
                        onFreeChange={(free) => {
                            setFilter({
                                ...filter,
                                free,
                            });
                        }}
                    />
                    <CourseLevelFilter
                        disabled={isLoadingCourses}
                        level={filter.level}
                        onLevelChange={(level) => {
                            setFilter({
                                ...filter,
                                level,
                            });
                        }}
                    />
                </div>
            </Drawer>
            <div className="my-5">
                <div className="flex justify-between gap-3 mb-5">
                    <div className="flex gap-3">
                        <button
                            onClick={() => setIsFilterPanelOpen(true)}
                            type="button"
                            className="flex items-center gap-2 border border-gray-900 px-3 py-1 bg-transparent rounded-none text-gray-900"
                        >
                            <IoFilterSharp size={16} />
                            Filter
                        </button>
                        <Select
                            disabled={isLoadingCourses}
                            onChange={(value) => {
                                setSort(value || "Newest");
                            }}
                            value={sort}
                            label="Sort by"
                            className="flex items-center gap-2 border border-gray-900 px-3 py-1 bg-transparent rounded-none text-gray-900"
                        >
                            <Option value="Newest">Newest</Option>
                            <Option value="HighestRated">Highest rated</Option>
                            <Option value="PriceDesc">Price high to low</Option>
                            <Option value="PriceAsc">Price low to high</Option>
                        </Select>
                    </div>
                    <div className="md:block hidden">
                        {totalCourses} results
                    </div>
                </div>
                <div>
                    {isLoadingCourses ? (
                        <div className="flex justify-center">
                            <Loader />
                        </div>
                    ) : (
                        <CoursesGrid
                            courses={courses}
                            end={end}
                            next={getMoreCourses}
                        />
                    )}
                </div>
            </div>
        </>
    );
}
