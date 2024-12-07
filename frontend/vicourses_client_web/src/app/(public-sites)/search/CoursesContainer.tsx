"use client";

import { useEffect, useState } from "react";
import { IoFilterSharp } from "react-icons/io5";
import { Select, Option, Drawer, Typography } from "@material-tailwind/react";
import { Course, CourseLevel } from "@/libs/types/course";
import { searchCourses } from "@/services/api/course";
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

export default function CoursesContainer({ keyword }: { keyword?: string }) {
    const [isFilterPanelOpen, setIsFilterPanelOpen] = useState<boolean>(false);
    const [filter, setFilter] = useState<CourseFilterOptions>({});
    const [sort, setSort] = useState<string>("Relevance");
    
    const [isLoadingCourses, setIsLoadingCourses] = useState<boolean>(false);
    const [courses, setCourses] = useState<Course[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [totalCourses, setTotalCourses] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(false);

    const searchMoreCourses = async () => {
        const data = await searchCourses({
            skip: skip + limit,
            limit,
            ...filter,
            sort: sort as any,
            q: keyword,
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

            const data = await searchCourses({
                skip: 0,
                limit,
                ...filter,
                sort: sort as any,
                q: keyword,
            });

            if (data) {
                setTotalCourses(data.total);
                setSkip(0);
                setEnd(data.end);
                setCourses(data.items);
                setIsLoadingCourses(false);
            }
        })();
    }, [filter, sort, keyword]);

    return (
        <div className="my-5">
            <Drawer
                open={isFilterPanelOpen}
                onClose={() => setIsFilterPanelOpen(false)}
                className="p-4"
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
                            setSort(value || "Relevance");
                        }}
                        value={sort}
                        label="Sort by"
                        className="flex items-center gap-2 border border-gray-900 px-3 py-1 bg-transparent rounded-none text-gray-900"
                    >
                        <Option value="Relevance">Relevance</Option>
                        <Option value="Newest">Newest</Option>
                        <Option value="HighestRated">Highest rated</Option>
                    </Select>
                </div>
                <div className="md:block hidden">{totalCourses} results</div>
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
                        next={searchMoreCourses}
                    />
                )}
            </div>
        </div>
    );
}
