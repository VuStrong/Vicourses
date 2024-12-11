"use client";

import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { CourseDetail } from "@/libs/types/course";
import CoursePurchaseSection from "./CoursePurchaseSection";
import OpenPreviewVideoModalButton from "./OpenPreviewVideoModalButton";

export default function Sidebar({ course }: { course: CourseDetail }) {
    return (
        <aside className="w-[350px] shadow-xl bg-white">
            <div
                className="relative w-full border border-gray-700"
                style={{
                    aspectRatio: "calc(1 / 0.5625)",
                }}
            >
                <img
                    className="w-full h-full"
                    src={course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL}
                    alt={course.title}
                />

                <div className="absolute w-full h-full bg-black bg-opacity-20 top-0 left-0 flex justify-center items-center">
                    <OpenPreviewVideoModalButton course={course} />
                </div>
            </div>

            <div className="px-5">
                <CoursePurchaseSection course={course} />
            </div>
        </aside>
    );
}
