import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { CourseInWishlist } from "@/libs/types/wishlist";
import { Rating } from "@material-tailwind/react";
import Link from "next/link";
import AddToWishlistButton from "../common/AddToWishlistButton";

export default function WishlistedCourseCard({
    course,
}: {
    course: CourseInWishlist;
}) {
    return (
        <Link
            href={`/course/${course.titleCleaned}/${course.id}`}
            className="w-full lg:max-w-full lg:flex gap-4 hover:shadow-lg transition-shadow relative"
        >
            <div
                className="absolute right-0 bottom-0 mx-3"
                onClick={(e) => e.preventDefault()}
            >
                <AddToWishlistButton courseId={course.id} />
            </div>
            <div
                className="w-full lg:max-w-[200px] border border-gray-700 flex-none bg-cover rounded-t lg:rounded-t-none lg:rounded-l text-center overflow-hidden"
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
            <div className="w-full bg-white rounded-b py-4 lg:py-0 lg:mb-8 lg:rounded-b-none lg:rounded-r flex flex-col justify-between leading-normal">
                <div className="text-gray-900 font-bold line-clamp-2">
                    {course.title}
                </div>
                <div>
                    <div className="text-gray-700 text-sm line-clamp-1">
                        {course.user.name}
                    </div>
                    <div className="flex items-center gap-2 font-bold text-blue-gray-500 mb-3">
                        {course.rating}
                        <Rating value={Math.floor(course.rating)} readonly />
                    </div>
                    <div className="text-black font-medium">
                        {course.isPaid ? `$${course.price}` : "Free"}
                    </div>
                </div>
            </div>
        </Link>
    );
}
