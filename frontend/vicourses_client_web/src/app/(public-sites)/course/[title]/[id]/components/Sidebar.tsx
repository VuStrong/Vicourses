"use client";

import { useEffect, useState } from "react";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { CourseDetail } from "@/libs/types/course";
import OpenPreviewVideoModalButton from "./OpenPreviewVideoModalButton";
import CoursePurchaseSection from "./CoursePurchaseSection";

export default function Sidebar({ course }: { course: CourseDetail }) {
    const [style, setStyle] = useState<string>("mt-10");

    const handleScroll = () => {
        if (window.scrollY > 350) {
            setStyle("fixed top-10 ml-[40rem] xl:ml-[50rem] 2xl:ml-[60rem]");
        } else {
            setStyle("mt-10");
        }
    };

    useEffect(() => {
        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener("scroll", handleScroll);
    }, []);

    return (
        <aside className={`w-[20rem] xl:w-[22rem] shadow-xl bg-white h-fit hidden lg:block ${style}`}>
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
