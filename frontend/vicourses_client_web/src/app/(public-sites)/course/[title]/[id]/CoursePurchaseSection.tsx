"use client";

import { useMemo } from "react";
import { IoIosInfinite } from "react-icons/io";
import { CiHeart } from "react-icons/ci";
import {
    MdOutlineOndemandVideo,
    MdOutlinePlayLesson,
    MdOutlineQuiz,
} from "react-icons/md";
import { Button } from "@material-tailwind/react";
import { CourseDetail } from "@/libs/types/course";

export default function CoursePurchaseSection({
    course,
}: {
    course: CourseDetail;
}) {
    const totalDuration = useMemo(
        () => new Date(course.metrics.totalVideoDuration * 1000),
        [course.id]
    );

    return (
        <div className="py-5">
            <div className="text-black font-bold text-2xl mb-3">
                ${course.price}
            </div>
            <div className="flex gap-2 mb-3">
                <Button
                    type="button"
                    className="bg-primary flex justify-center items-center flex-grow rounded-none py-1"
                >
                    Buy now
                </Button>
                <Button
                    title="Add to wishlist"
                    type="button"
                    className="rounded-none bg-transparent text-black border border-gray-900 py-1 px-1"
                >
                    <CiHeart size={32} />
                </Button>
            </div>
            <div className="text-gray-700 text-sm text-center mb-5">
                2-day money-back guarantee
            </div>
            <div className="">
                <div className="text-black font-semibold mb-1">
                    This course contains:
                </div>
                <div className="text-gray-800">
                    <div className="flex gap-5 items-center">
                        <MdOutlineOndemandVideo size={16} />
                        {totalDuration.getUTCHours() > 0
                            ? `${totalDuration.getUTCHours()} hours of video on demand`
                            : `${totalDuration.getUTCMinutes()} minutes of video on demand`}
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
