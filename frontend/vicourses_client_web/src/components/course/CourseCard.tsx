import { Rating } from "@material-tailwind/react";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { Course } from "@/libs/types/course";
import Link from "next/link";

export default function CourseCard({ course }: { course: Course }) {
    return (
        <Link href="#" className="flex gap-3 justify-between">
            <img
                className="w-1/3 object-cover object-center border border-gray-700"
                src={course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL}
                alt={course.title}
            />
            <div className="flex-grow">
                <div className="text-black font-bold line-clamp-2">{course.title}</div>
                <div className="text-gray-700 text-sm">{course.user.name}</div>
                <div className="flex items-center gap-2 font-bold text-blue-gray-500">
                    {course.rating}
                    <Rating value={Math.floor(course.rating)} readonly />
                </div>
                <div className="text-gray-700 text-base">
                    {course.level}
                </div>
            </div>
            <div className="text-black font-bold">
                ${course.price}
            </div>
        </Link>
    );
}
