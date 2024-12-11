"use client";

import { Loader } from "@/components/common";
import CoursesGrid from "@/components/course/CoursesGrid";
import { Course } from "@/libs/types/course";
import { getUserEnrolledCourses } from "@/services/api/course";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";

const limit = 12;

export default function MyCoursesPage() {
    const [isLoading, setIsLoading] = useState<boolean>(false);
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
        <CoursesGrid courses={courses} end={end} next={getMoreCourses} />
    );
}
