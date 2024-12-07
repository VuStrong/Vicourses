"use client";

import { Button } from "@material-tailwind/react";
import { Course } from "@/libs/types/course";
import CourseCard from "./CourseCard";
import { useState } from "react";

export default function CoursesGrid({
    courses,
    next,
    end,
}: {
    courses: Course[];
    next: () => Promise<void>;
    end: boolean;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <>
            {courses.length === 0 && (
                <div className="flex justify-center text-gray-900 font-bold">
                    No results
                </div>
            )}

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-3">
                {courses.map((course) => (
                    <CourseCard key={course.id} course={course} />
                ))}
            </div>
            {!end && (
                <div className="flex items-center justify-center mt-5">
                    <Button
                        loading={isLoading}
                        className="bg-transparent text-gray-900 border border-gray-900"
                        onClick={async () => {
                            setIsLoading(true);
                            await next();
                            setIsLoading(false);
                        }}
                    >
                        Load more
                    </Button>
                </div>
            )}
        </>
    );
}
