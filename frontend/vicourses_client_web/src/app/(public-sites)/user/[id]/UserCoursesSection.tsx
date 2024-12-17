"use client";

import { Loader } from "@/components/common";
import CoursesGrid from "@/components/course/CoursesGrid";
import { Course } from "@/libs/types/course";
import {
    getInstructorCourses,
    getUserEnrolledCourses,
} from "@/services/api/course";
import { useEffect, useState } from "react";

const limit = 8;

export default function UserCoursesSection({ userId }: { userId: string }) {
    const [tab, setTab] = useState<number>(0);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [courses, setCourses] = useState<Course[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [totalCourses, setTotalCourses] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(false);

    const getMoreCourses = async () => {
        const data =
            tab === 0
                ? await getInstructorCourses({
                      instructorId: userId,
                      skip: skip + limit,
                      limit,
                  })
                : await getUserEnrolledCourses({
                      userId,
                      skip: skip + limit,
                      limit,
                  });

        if (data) {
            setCourses([...courses, ...data.items]);
            setSkip(skip + limit);
            setEnd(data.end);
        }
    };

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const data =
                tab === 0
                    ? await getInstructorCourses({
                          instructorId: userId,
                          skip: 0,
                          limit,
                      })
                    : await getUserEnrolledCourses({
                          userId,
                          skip: 0,
                          limit,
                      });

            if (data) {
                setTotalCourses(data.total);
                setSkip(0);
                setEnd(data.end);
                setCourses(data.items);
            } else {
                setCourses([]);
            }
            
            setIsLoading(false);
        })();
    }, [tab]);

    return (
        <div>
            <div className="flex flex-nowrap items-center overflow-x-auto text-gray-700 font-semibold mb-5">
                <div
                    className={`py-2 pr-5 hover:opacity-80 border-gray-700 whitespace-nowrap cursor-pointer ${
                        tab === 0 && "border-b-4"
                    }`}
                    onClick={() => setTab(0)}
                >
                    My courses {tab === 0 && `(${totalCourses})`}
                </div>
                <div
                    className={`py-2 px-5 hover:opacity-80 border-gray-700 cursor-pointer ${
                        tab === 1 && "border-b-4"
                    }`}
                    onClick={() => setTab(1)}
                >
                    Enrolled courses {tab === 1 && `(${totalCourses})`}
                </div>
            </div>
            {isLoading ? (
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
    );
}
