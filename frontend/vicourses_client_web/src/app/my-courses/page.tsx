"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { Button } from "@material-tailwind/react";

import { Loader } from "@/components/common";
import LearningUnitCard from "@/components/course/LearningUnitCard";
import { Course } from "@/libs/types/course";
import { getUserEnrolledCourses } from "@/services/api/course";

const limit = 12;

export default function MyCoursesPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [courses, setCourses] = useState<Course[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(false);
    const { data: session, status } = useSession();

    const getMoreCourses = async () => {
        if (status === "authenticated") {
            const data = await getUserEnrolledCourses(
                {
                    userId: session.user.id,
                    skip: skip + limit,
                    limit,
                },
                session.accessToken
            );

            if (data) {
                setCourses([...courses, ...data.items]);
                setSkip(skip + limit);
                setEnd(data.end);
            }
        }
    };

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const data = await getUserEnrolledCourses(
                    {
                        userId: session.user.id,
                        limit,
                    },
                    session.accessToken
                );

                if (data) {
                    setSkip(0);
                    setEnd(data.end);
                    setCourses(data.items);
                    setIsLoading(false);
                }
            })();
        }
    }, [status]);

    return isLoading ? (
        <div className="flex justify-center">
            <Loader />
        </div>
    ) : (
        <LearningList courses={courses} end={end} next={getMoreCourses} />
    );
}

function LearningList({
    courses,
    next,
    end,
}: {
    courses: Course[];
    next?: () => Promise<void>;
    end: boolean;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <>
            {courses.length === 0 && (
                <div className="flex justify-center text-gray-900 font-bold">
                    You don't have any courses
                </div>
            )}

            <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                {courses.map((course) => (
                    <LearningUnitCard key={course.id} course={course} />
                ))}
            </div>
            {next && !end && (
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
