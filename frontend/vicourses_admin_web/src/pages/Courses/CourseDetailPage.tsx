import { useEffect, useMemo, useState } from "react";
import { Link, useParams } from "react-router-dom";
import {
    MdOutlineOndemandVideo,
    MdOutlinePlayLesson,
    MdOutlineQuiz,
    MdOutlineUpdate,
} from "react-icons/md";
import { CiGlobe } from "react-icons/ci";
import { IoIosInfinite } from "react-icons/io";
import { FcApproval } from "react-icons/fc";

import { CourseDetail } from "../../types/course";
import axiosInstance from "../../libs/axios";
import Loader from "../../components/Loader";
import Rating from "../../components/Rating";
import CourseLearnedContents from "../../components/Course/CourseLearnedContents";
import CoursePreviewVideo from "../../components/Course/CoursePreviewVideo";
import CourseCurriculum from "../../components/Course/CourseCurriculum";
import CourseActions from "../../components/Course/CourseActions";

export default function CourseDetailPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [course, setCourse] = useState<CourseDetail | null>(null);

    const params = useParams();

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<CourseDetail>(`/api/cs/v1/courses/${params.id}`)
            .then((response) => {
                setCourse(response.data);
                setIsLoading(false);
            })
            .catch((error) => {
                if (error?.response?.status === 404) {
                    setIsLoading(false);
                }
            });
    }, [params.id]);

    const courseDurationString = useMemo(() => {
        if (!course) return "";

        const time = new Date(course.metrics.totalVideoDuration * 1000);

        return time.getUTCHours() > 0
            ? `${time.getUTCHours()} hours of video`
            : `${time.getUTCMinutes()} minutes of video`;
    }, [course?.id]);

    return (
        <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
            {isLoading && (
                <div className="flex justify-center">
                    <Loader />
                </div>
            )}

            {!isLoading && !course && (
                <div className="text-center">Course not found</div>
            )}

            {!isLoading && course && (
                <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                    {course.isApproved && (
                        <div title="Approved" className="mb-3">
                            <FcApproval size={36} />
                        </div>
                    )}

                    <div className="md:flex gap-3">
                        <div className="w-full md:max-w-[40%] mb-5">
                            <CoursePreviewVideo course={course} />
                        </div>
                        <div>
                            <h3 className="text-xl md:text-2xl font-bold text-black dark:text-white mb-3">
                                {course.title}
                            </h3>
                            <div className="flex gap-3 mb-3">
                                {course.rating}
                                <Rating value={course.rating} />
                                {course.studentCount} students
                            </div>
                            <div className="mb-2">
                                Created by{" "}
                                <Link
                                    to={`/users/${course.user.id}`}
                                    className="underline text-primary font-bold"
                                >
                                    {course.user.name}
                                </Link>
                            </div>
                            <div className="flex gap-2 items-center mb-2 text-sm">
                                <div className="flex gap-2 items-center">
                                    <MdOutlineUpdate size={14} /> Last updated{" "}
                                    {new Date(
                                        course.updatedAt,
                                    ).toLocaleDateString()}
                                </div>
                                <div className="flex gap-2 items-center">
                                    <CiGlobe size={14} />
                                    {course.locale?.englishTitle}
                                </div>
                            </div>
                            <div>
                                <div className="text-boxdark dark:text-white font-semibold">
                                    Tags:
                                </div>
                                <div className="flex flex-wrap gap-3">
                                    {course.tags.map((tag) => (
                                        <div
                                            key={tag}
                                            className="rounded-full px-2 bg-primary/20"
                                        >
                                            {tag}
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="my-5">
                        <CourseActions
                            course={course}
                            onCourseApprovalCanceled={() => {
                                setCourse({
                                    ...course,
                                    isApproved: false,
                                    status: "Unpublished",
                                });
                            }}
                            onCourseApproved={() => {
                                setCourse({
                                    ...course,
                                    isApproved: true,
                                    status: "Published",
                                });
                            }}
                        />
                    </div>

                    <div>
                        <div className="mb-5">
                            <div>
                                <span className="font-semibold">Status:</span>{" "}
                                {course.status}
                            </div>
                            <div>
                                <span className="font-semibold">Price:</span>{" "}
                                {course.isPaid ? `$${course.price}` : "Free"}
                            </div>
                            <div>
                                <span className="font-semibold">Category:</span>{" "}
                                {course.category.name}
                            </div>
                            <div>
                                <span className="font-semibold">
                                    Sub category:
                                </span>{" "}
                                {course.subCategory.name}
                            </div>
                            <div>
                                <span className="font-semibold">Level:</span>{" "}
                                {course.level}
                            </div>
                        </div>
                        <div className="md:flex gap-5">
                            <div className="flex-shrink-0 mb-5 md:mb-0">
                                <div className="font-semibold mb-1">
                                    This course contains:
                                </div>
                                <div className="flex gap-3 items-center">
                                    <MdOutlineOndemandVideo size={16} />
                                    {courseDurationString}
                                </div>
                                <div className="flex gap-3 items-center">
                                    <MdOutlinePlayLesson size={16} />
                                    {course.metrics.lessonsCount} lessons
                                </div>
                                <div className="flex gap-3 items-center">
                                    <MdOutlineQuiz size={16} />
                                    {course.metrics.quizLessonsCount} quizzes
                                </div>
                                <div className="flex gap-3 items-center">
                                    <IoIosInfinite size={16} />
                                    Full lifetime access
                                </div>
                            </div>

                            <CourseLearnedContents
                                contents={course.learnedContents}
                            />
                        </div>

                        <section className="mt-7">
                            <h2 className="font-semibold text-2xl mb-4">
                                Requirements
                            </h2>

                            <ul className="list-disc pl-5">
                                {course.requirements?.map(
                                    (requiment, index) => (
                                        <li key={index}>{requiment}</li>
                                    ),
                                )}
                            </ul>
                        </section>

                        {course.description && (
                            <section className="mt-7">
                                <h2 className="font-semibold text-2xl mb-4">
                                    Description
                                </h2>

                                <div
                                    className="[&_:is(ol,ul)]:[list-style:revert] [&_:is(ol,ul)]:[margin:revert] [&_:is(ol,ul)]:[padding:revert]"
                                    dangerouslySetInnerHTML={{
                                        __html: course.description,
                                    }}
                                />
                            </section>
                        )}

                        <section className="mt-7">
                            <h2 className="font-semibold text-2xl mb-4">
                                Target students
                            </h2>

                            <ul className="list-disc pl-5">
                                {course.targetStudents?.map((target, index) => (
                                    <li key={index}>{target}</li>
                                ))}
                            </ul>
                        </section>

                        <div className="mt-10">
                            <h2 className="font-semibold text-2xl mb-4">
                                Curriculum
                            </h2>

                            <CourseCurriculum courseId={course.id} />
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
