"use client";

import { useMemo } from "react";
import { useRouter } from "next/navigation";
import { IoIosInfinite } from "react-icons/io";
import {
    MdOutlineOndemandVideo,
    MdOutlinePlayLesson,
    MdOutlineQuiz,
} from "react-icons/md";
import { FaInfoCircle } from "react-icons/fa";
import { Button } from "@material-tailwind/react";

import { CourseDetail } from "@/libs/types/course";
import { Loader } from "@/components/common";
import AddToWishlistButton from "@/components/common/AddToWishlistButton";
import useEnrollStatus from "../hooks/useEnrollStatus";
import EnrollButton from "./EnrollButton";

export default function CoursePurchaseSection({
    course,
}: {
    course: CourseDetail;
}) {
    const isLoading = useEnrollStatus(state => state.isLoading);
    const enrolled = useEnrollStatus(state => state.enrolled);
    const router = useRouter();

    const time = useMemo(
        () => new Date(course.metrics.totalVideoDuration * 1000),
        [course.id]
    );

    return (
        <div className="py-5">
            <div className="text-black font-bold text-2xl mb-3">
                {course.isPaid ? `$${course.price}` : "Free"}
            </div>

            {isLoading && (
                <div className="flex justify-center">
                    <Loader />
                </div>
            )}

            {!isLoading &&
                (enrolled ? (
                    <>
                        <div className="flex gap-2 text-primary">
                            <FaInfoCircle size={20} />
                            <div className="text-black flex-grow font-bold">
                                You enrolled this course
                            </div>
                        </div>
                        <Button
                            type="button"
                            className="rounded-none mb-3"
                            fullWidth
                            onClick={() =>
                                router.push(`/learn/course/${course.id}`)
                            }
                        >
                            Go to learn
                        </Button>
                    </>
                ) : course.isPaid ? (
                    <>
                        <div className="flex gap-2 mb-3">
                            <Button
                                type="button"
                                className="bg-primary flex justify-center items-center flex-grow rounded-none py-1"
                                onClick={() =>
                                    router.push(`/checkout/course/${course.id}`)
                                }
                            >
                                Buy now
                            </Button>
                            <AddToWishlistButton courseId={course.id} />
                        </div>
                        <div className="text-gray-700 text-sm text-center mb-5">
                            2-day money-back guarantee
                        </div>
                    </>
                ) : (
                    <div className="mb-3">
                        <EnrollButton course={course} />
                    </div>
                ))}

            <div>
                <div className="text-black font-semibold mb-1">
                    This course contains:
                </div>
                <div className="text-gray-800">
                    <div className="flex gap-5 items-center">
                        <MdOutlineOndemandVideo size={16} />
                        {time.getUTCHours() > 0
                            ? `${time.getUTCHours()} hours of video on demand`
                            : `${time.getUTCMinutes()} minutes of video on demand`}
                    </div>
                    <div className="flex gap-5 items-center">
                        <MdOutlinePlayLesson size={16} />
                        {course.metrics.lessonsCount} lessons
                    </div>
                    <div className="flex gap-5 items-center">
                        <MdOutlineQuiz size={16} />
                        {course.metrics.quizLessonsCount} quizzes
                    </div>
                    <div className="flex gap-5 items-center">
                        <IoIosInfinite size={16} />
                        Full lifetime access
                    </div>
                </div>
            </div>
        </div>
    );
}
