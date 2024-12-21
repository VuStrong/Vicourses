import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";

import {
    DEFAULT_COURSE_IMAGE_URL,
    DEFAULT_USER_AVATAR_URL,
} from "../../libs/contants";
import axiosInstance from "../../libs/axios";
import { Course, CourseStatus } from "../../types/course";
import { PagedResult } from "../../types/common";
import Loader from "../../components/Loader";
import SearchBar from "../../components/SearchBar";
import { Option, Select } from "../../components/Forms";

const pageSize = 10;

export default function CoursesPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [courses, setCourses] = useState<Course[]>([]);
    const [total, setTotal] = useState<number>(0);

    const location = useLocation();
    const navigate = useNavigate();
    const queryParams = new URLSearchParams(location.search);

    const currentPage = Number(queryParams.get("page")) || 1;
    const totalPage = Math.ceil(total / pageSize);
    const searchValue = queryParams.get("q");
    const courseStatus =
        (queryParams.get("status") as CourseStatus) || "Published";

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<PagedResult<Course>>(`/api/cs/v1/courses`, {
                params: {
                    skip: (currentPage - 1) * pageSize,
                    limit: pageSize,
                    sort: "Newest",
                    status: courseStatus,
                    q: searchValue,
                },
            })
            .then((response) => {
                setCourses(response.data.items);
                setTotal(response.data.total);
                setIsLoading(false);
            });
    }, [currentPage, courseStatus, searchValue]);

    const handleChangePage = (page: number) => {
        if (page === currentPage) return;

        queryParams.set("page", `${page}`);
        navigate({ search: queryParams.toString() });
    };

    const handleChangeStatus = (status: CourseStatus) => {
        if (status === courseStatus) return;

        queryParams.set("status", status);
        queryParams.delete("page");
        navigate({ search: queryParams.toString() });
    };

    const handleChangeSearchValue = (value: string) => {
        if (value === searchValue) return;

        queryParams.set("q", value);
        queryParams.delete("page");
        navigate({ search: queryParams.toString() });
    };

    return (
        <>
            <div className="flex items-center justify-between flex-wrap">
                <h1 className="text-2xl font-bold mb-5">Courses ({total})</h1>
            </div>
            <div className="flex gap-3 flex-wrap mb-5">
                <SearchBar
                    placeholder="Search courses"
                    onSubmit={(value) => handleChangeSearchValue(value)}
                    disabled={isLoading}
                />
                <Select
                    value={courseStatus}
                    onChange={(e) =>
                        handleChangeStatus(e.target.value as CourseStatus)
                    }
                    disabled={isLoading}
                >
                    <Option value="Unpublished">Unpublished</Option>
                    <Option value="WaitingToVerify">Waiting To Verify</Option>
                    <Option value="Published">Published</Option>
                </Select>
            </div>
            <div>
                {isLoading && (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                )}

                {!isLoading &&
                    (courses.length === 0 ? (
                        <div className="text-center">No course found</div>
                    ) : (
                        <>
                            <CoursesTable courses={courses} />
                            <div className="flex mt-5">
                                <button
                                    disabled={currentPage <= 1}
                                    onClick={() =>
                                        handleChangePage(currentPage - 1)
                                    }
                                    className="flex items-center justify-center px-3 h-8 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 disabled:opacity-50"
                                >
                                    Previous
                                </button>
                                <button
                                    disabled={currentPage >= totalPage}
                                    onClick={() =>
                                        handleChangePage(currentPage + 1)
                                    }
                                    className="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 disabled:opacity-50"
                                >
                                    Next
                                </button>
                            </div>
                        </>
                    ))}
            </div>
        </>
    );
}

function CoursesTable({ courses }: { courses: Course[] }) {
    return (
        <div className="rounded-sm border border-stroke bg-white pt-2 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark xl:pb-1">
            <div className="max-w-full overflow-x-auto">
                <table className="w-full min-w-max table-auto text-left">
                    <thead>
                        <tr>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Title
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Instructor
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Created At
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Status
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {courses.map((course, index) => {
                            return (
                                <tr key={index}>
                                    <td className="p-4">
                                        <div className="flex items-center gap-3">
                                            <div className="h-12.5 w-15 rounded-md">
                                                <img
                                                    src={
                                                        course.thumbnailUrl ||
                                                        DEFAULT_COURSE_IMAGE_URL
                                                    }
                                                    alt={course.title}
                                                    className="w-full h-full object-cover"
                                                />
                                            </div>
                                            <Link
                                                to={`/courses/${course.id}`}
                                                className="hover:text-primary"
                                            >
                                                {course.title}
                                            </Link>
                                        </div>
                                    </td>
                                    <td className="p-4">
                                        <div className="flex items-center gap-3">
                                            <div className="h-10 w-10 rounded-full">
                                                <img
                                                    src={
                                                        course.user
                                                            .thumbnailUrl ||
                                                        DEFAULT_USER_AVATAR_URL
                                                    }
                                                    alt={course.user.name}
                                                    className="w-full h-full object-cover"
                                                />
                                            </div>
                                            <p className="max-w-[100px] line-clamp-1">
                                                {course.user.name}
                                            </p>
                                        </div>
                                    </td>
                                    <td className="p-4">{course.createdAt}</td>
                                    <td className="p-4">
                                        <p
                                            className={`inline-flex rounded-full bg-opacity-10 text-sm font-medium ${
                                                course.status === "Published"
                                                    ? "bg-success text-success"
                                                    : course.status ===
                                                      "Unpublished"
                                                    ? ""
                                                    : "bg-warning text-warning"
                                            }`}
                                        >
                                            {course.status}
                                        </p>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
