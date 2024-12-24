import { Course } from "@/libs/types/course";
import Link from "next/link";
import { MdArrowBackIos } from "react-icons/md";

export default function CourseManagementHeader({ course }: { course: Course }) {
    return (
        <header className="flex gap-3 items-center fixed top-0 w-full bg-black p-5 z-[10]">
            <Link
                href="/instructor/courses"
                className="text-gray-300 flex items-center hover:opacity-50 gap-2"
            >
                <MdArrowBackIos size={16} />
                <div className="hidden md:block">Back to courses</div>
            </Link>
            <div className="text-white font-semibold line-clamp-1 flex-grow">
                {course.title}
            </div>
            <a
                href={`/course/${course.titleCleaned}/${course.id}`}
                target="_blank"
                className="text-gray-300 border border-gray-300 py-1 px-3"
            >
                Preview
            </a>
        </header>
    );
}
