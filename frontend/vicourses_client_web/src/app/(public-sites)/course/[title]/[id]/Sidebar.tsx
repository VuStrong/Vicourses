"use client";

import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { CourseDetail } from "@/libs/types/course";
import CoursePurchaseSection from "./CoursePurchaseSection";

export default function Sidebar({ course }: { course: CourseDetail }) {
    return (
        <aside className="w-[350px] shadow-xl bg-white">
            <div
                className="w-full border border-gray-700"
                style={{
                    aspectRatio: "calc(1 / 0.5625)",
                }}
            >
                <img
                    className="w-full h-full"
                    src={course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL}
                    alt={course.title}
                />
            </div>

            <div className="px-5">
                <CoursePurchaseSection course={course} />
            </div>
        </aside>
    );
}
