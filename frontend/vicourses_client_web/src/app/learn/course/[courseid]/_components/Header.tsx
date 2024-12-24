import Link from "next/link";
import { Course } from "@/libs/types/course";

export default function Header({ course }: { course: Course }) {
    return (
        <header className="flex gap-3 items-center w-full bg-black p-5 border-b border-gray-800">
            <Link
                href="/"
                className="text-gray-300 border-r border-gray-700 pr-5 font-bold"
            >
                Home
            </Link>
            <div className="text-gray-300 flex-grow line-clamp-1">
                {course.title}
            </div>
        </header>
    );
}
