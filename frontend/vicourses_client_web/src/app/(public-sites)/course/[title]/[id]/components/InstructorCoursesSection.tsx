"use client";

import { useEffect, useState } from "react";
import { Course, CourseDetail } from "@/libs/types/course";
import { getInstructorCourses } from "@/services/api/course";
import CoursesGrid from "@/components/course/CoursesGrid";
import { Loader } from "@/components/common";

export default function InstructorCoursesSection({
    course,
}: {
    course: CourseDetail;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [courses, setCourses] = useState<Course[]>([]);

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const result = await getInstructorCourses({
                instructorId: course.user.id,
                limit: 4,
            });

            if (result?.items) {
                setCourses(result.items.filter(c => c.id !== course.id));
            }
            setIsLoading(false);
        })();
    }, []);

    if (courses.length === 0) {
        return null;
    }

    return (
        <section className="mt-10">
            <h2 className="text-black font-semibold text-2xl mb-4">
                Other courses of{" "}
                <span className="text-primary">{course.user.name}</span>
            </h2>

            {isLoading ? (
                <div className="flex justify-center">
                    <Loader />
                </div>
            ) : (
                <CoursesGrid courses={courses} end />
            )}
        </section>
    );
}
