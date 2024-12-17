"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { useSession } from "next-auth/react";

import { Course } from "@/libs/types/course";
import { getUserEnrolledCourses } from "@/services/api/course";
import { Loader } from "@/components/common";
import LearningUnitCard from "@/components/course/LearningUnitCard";

export default function UserCoursesSection() {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [courses, setCourses] = useState<Course[]>([]);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getUserEnrolledCourses(
                    {
                        userId: session.user.id,
                        limit: 3,
                    },
                    session.accessToken
                );

                setCourses(result?.items || []);
                setIsLoading(false);
            })();
        }
    }, [status]);

    if (!isLoading && courses.length === 0) {
        return null;
    }

    return (
        <div className="mt-5 mb-10">
            <div className="flex items-center justify-between">
                <h2 className="text-black font-bold text-2xl mb-3">
                    Let's start learning
                </h2>
                <Link
                    href="/my-courses"
                    className="text-primary font-bold underline"
                >
                    Learn
                </Link>
            </div>
            {isLoading && (
                <div className="flex justify-center">
                    <Loader />
                </div>
            )}
            {!isLoading && (
                <section className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3">
                    {courses.map((course) => (
                        <LearningUnitCard key={course.id} course={course} />
                    ))}
                </section>
            )}
        </div>
    );
}
