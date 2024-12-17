import Link from "next/link";
import { FaPlay } from "react-icons/fa";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { Course } from "@/libs/types/course";

export default function LearningUnitCard({ course }: { course: Course }) {
    return (
        <Link
            href={`/learn/course/${course.id}`}
            className="flex gap-2 md:gap-4 hover:shadow-lg transition-shadow border border-gray-300"
        >
            <div className="aspect-square max-w-32 border border-gray-300 flex-none bg-cover overflow-hidden relative">
                <img
                    className="w-full h-full object-cover"
                    src={course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL}
                    alt={course.title}
                />
                <div className="absolute inset-0 bg-black/50 flex justify-center items-center text-white">
                    <FaPlay size={32} />
                </div>
            </div>
            <div className="flex-grow bg-white lg:mb-8 flex flex-col leading-normal">
                <div className="text-gray-900 font-bold line-clamp-2">
                    {course.title}
                </div>
                <div>
                    <div className="text-gray-700 text-sm line-clamp-1">
                        {course.user.name}
                    </div>
                </div>
            </div>
        </Link>
    );
}
