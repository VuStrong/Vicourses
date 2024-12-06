"use client";

import { Course } from "@/libs/types/course";
import InfiniteScroll from "react-infinite-scroll-component";
import { Loader } from "../common";
import CourseCard from "./CourseCard";

export default function CourseInfinityScrollList({
    courses,
    skip,
    limit,
    next,
    end,
}: {
    courses: Course[];
    skip: number;
    limit: number;
    next: () => void;
    end: boolean;
}) {
    return (
        <InfiniteScroll
            dataLength={skip + limit}
            next={next}
            hasMore={!end}
            loader={
                <div className="flex justify-center">
                    <Loader />
                </div>
            }
            style={{
                overflow: "unset"
            }}
        >
            <div className="flex flex-col gap-3">
                {courses.map(course => (
                    <CourseCard key={course.id} course={course} />
                ))}
            </div>
        </InfiniteScroll>
    );
}
