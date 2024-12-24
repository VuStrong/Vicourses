"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import Link from "next/link";
import { IoMdSearch } from "react-icons/io";
import { Select, Option, Button } from "@material-tailwind/react";

import { Loader } from "@/components/common";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { Course, CourseStatus } from "@/libs/types/course";
import { getInstructorCourses } from "@/services/api/course";
import CreateCourseButton from "./CreateCourseButton";

const limit = 5;

export default function CoursesContainer() {
    const [searchValue, setSearchValue] = useState<string>("");
    const [courseStatus, setCourseStatus] = useState<CourseStatus>();

    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [courses, setCourses] = useState<Course[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);
    const { data: session, status } = useSession();

    const getMoreCourses = async () => {
        if (status === "authenticated") {
            const result = await getInstructorCourses(
                {
                    instructorId: session.user.id,
                    q: searchValue ? searchValue : undefined,
                    status: courseStatus,
                    skip: skip + limit,
                    limit,
                },
                session.accessToken
            );

            if (result) {
                setCourses([...courses, ...result.items]);
                setEnd(result.end);
                setSkip(skip + limit);
                setIsLoading(false);
            }
        }
    };

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getInstructorCourses(
                    {
                        instructorId: session.user.id,
                        q: searchValue ? searchValue : undefined,
                        status: courseStatus,
                        limit,
                    },
                    session.accessToken
                );

                if (result) {
                    setCourses(result.items);
                    setEnd(result.end);
                    setSkip(0);
                    setIsLoading(false);
                }
            })();
        }
    }, [searchValue, courseStatus, status]);

    return (
        <section>
            <div className="flex flex-col items-start sm:flex-row gap-3 justify-between sm:items-center flex-wrap mb-5">
                <div className="flex flex-col sm:flex-row gap-3 items-center">
                    <SearchBar onSubmit={(value) => setSearchValue(value)} />
                    <Select
                        onChange={(value) =>
                            setCourseStatus(
                                !!value ? (value as CourseStatus) : undefined
                            )
                        }
                        value={courseStatus?.toString()}
                        label="Status"
                        className="flex flex-1 items-center gap-2 border border-gray-900 px-3 py-1 bg-transparent rounded-none text-gray-900"
                    >
                        <Option value="">All</Option>
                        <Option value="Unpublished">Unpublished</Option>
                        <Option value="WaitingToVerify">
                            Waiting To Verify
                        </Option>
                        <Option value="Published">Published</Option>
                    </Select>
                </div>
                <div>
                    <CreateCourseButton />
                </div>
            </div>
            <div>
                {isLoading ? (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                ) : (
                    <>
                        {!courses[0] && (
                            <div className="flex justify-center text-gray-900 font-bold">
                                {!searchValue && !courseStatus
                                    ? "You have no courses"
                                    : "No results"}
                            </div>
                        )}
                        <CourseList
                            courses={courses}
                            end={end}
                            next={getMoreCourses}
                        />
                    </>
                )}
            </div>
        </section>
    );
}

function SearchBar({ onSubmit }: { onSubmit: (value: string) => void }) {
    const [value, setValue] = useState<string>("");

    return (
        <form
            onSubmit={(e) => {
                e.preventDefault();
                onSubmit(value);
            }}
            className="flex border border-gray-600 h-10"
        >
            <input
                type="search"
                placeholder="Search your courses"
                value={value}
                onChange={(e) => setValue(e.target.value)}
                className="flex-grow border-none outline-none p-3"
            />

            <button className="bg-black text-white px-3" type="submit">
                <IoMdSearch size={24} />
            </button>
        </form>
    );
}

function CourseList({
    courses,
    end,
    next,
}: {
    courses: Course[];
    end: boolean;
    next: () => Promise<void>;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <>
            <div className="flex flex-col gap-3">
                {courses.map((course) => (
                    <CourseItem key={course.id} course={course} />
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

function CourseItem({ course }: { course: Course }) {
    return (
        <Link
            href={`/instructor/courses/${course.id}`}
            className="flex gap-3 border border-gray-700"
        >
            <div className="max-w-[120px] md:max-w-[200px] max-h-[120px] border border-gray-700 flex-shrink-0">
                <img
                    className="w-full h-full aspect-square object-cover"
                    src={course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL}
                    alt={course.title}
                />
            </div>
            <div>
                <div className="text-black font-semibold line-clamp-2">
                    {course.title}
                </div>
                <div className="text-gray-800">
                    Created at {new Date(course.createdAt).toLocaleDateString()}
                </div>
                <div>
                    {course.status === "WaitingToVerify"
                        ? "Verifing"
                        : course.status}
                </div>
            </div>
        </Link>
    );
}
