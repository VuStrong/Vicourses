"use client";

import { useEffect, useState } from "react";
import { IoFilterSharp } from "react-icons/io5";
import { Select, Option } from "@material-tailwind/react";
import { Category } from "@/libs/types/category";
import { Course } from "@/libs/types/course";
import CourseFilters, {
    CourseFilterOptions,
} from "@/components/course/CourseFilters";
import CourseInfinityScrollList from "@/components/course/CourseInfinityScrollList";
import { getCourses } from "@/services/api/course";
import { Loader } from "@/components/common";

const limit = 15;

export default function CoursesContainer({ category }: { category: Category }) {
    const isRootCategory = category.parentId === null;

    const [sort, setSort] = useState<string>("Newest");
    const [isLoadingCourses, setIsLoadingCourses] = useState<boolean>(false);
    const [filter, setFilter] = useState<CourseFilterOptions>({});
    const [courses, setCourses] = useState<Course[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [totalCourses, setTotalCourses] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(false);

    const getMoreCourses = async () => {
        const data = await getCourses({
            skip: skip + limit,
            limit,
            free: filter.free,
            level: filter.level,
            rating: filter.rating,
            sort: sort as any,
            categoryId: isRootCategory ? category.id : undefined,
            subCategoryId: isRootCategory ? undefined : category.id,
        });

        if (data) {
            setCourses([...courses, ...data.items]);
            setSkip(skip + limit);
            setEnd(data.end);
        }
    }

    useEffect(() => {
        (async () => {
            setIsLoadingCourses(true);

            const data = await getCourses({
                skip: 0,
                limit,
                free: filter.free,
                level: filter.level,
                rating: filter.rating,
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
        <div className="my-5">
            <div className="flex justify-between gap-3 mb-5">
                <div className="flex gap-3">
                    <button
                        type="button"
                        className="flex items-center gap-2 border border-gray-900 px-3 py-1 bg-transparent rounded-none text-gray-900"
                    >
                        <IoFilterSharp size={16} />
                        Filter
                    </button>
                    <Select
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
                <div className="md:block hidden">{totalCourses} results</div>
            </div>
            <div className="flex gap-5">
                <div className="min-w-[310px] lg:block hidden">
                    <CourseFilters
                        filter={filter}
                        onFilterChange={(value) => {
                            setFilter(value);
                        }}
                    />
                </div>
                <div className="flex-grow">
                    {isLoadingCourses ? (
                        <div className="flex justify-center">
                            <Loader />
                        </div>
                    ) : (
                        <CourseInfinityScrollList 
                            courses={courses}
                            skip={skip}
                            limit={limit}
                            end={end}
                            next={getMoreCourses}
                        />
                    )}
                </div>
            </div>
        </div>
    );
}
