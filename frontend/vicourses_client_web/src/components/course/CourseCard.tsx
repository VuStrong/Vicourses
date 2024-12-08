import Link from "next/link";
import { Rating } from "@material-tailwind/react";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { Course } from "@/libs/types/course";
import AddToWishlistButton from "../common/AddToWishlistButton";

export default function CourseCard({ course }: { course: Course }) {
    return (
        <Link
            href={`/course/${course.titleCleaned}/${course.id}`}
            className="hover:shadow-xl transition-all relative"
        >
            <div
                className="absolute bottom-0 right-0 mx-3"
                onClick={(e) => e.preventDefault()}
            >
                <AddToWishlistButton courseId={course.id} />
            </div>

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
            <div className="p-2">
                <div className="text-black font-bold line-clamp-2">
                    {course.title}
                </div>
                <div className="text-gray-700 text-sm line-clamp-1">
                    {course.user.name}
                </div>
                <div className="flex items-center gap-2 font-bold text-blue-gray-500 py-2">
                    {course.rating}
                    <Rating value={Math.floor(course.rating)} readonly />
                </div>
                <div className="text-black font-medium">
                    {course.isPaid ? `$${course.price}` : "Free"}
                </div>
            </div>
        </Link>
    );
}
