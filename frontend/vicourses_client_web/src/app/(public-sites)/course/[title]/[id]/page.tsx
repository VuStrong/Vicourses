import { Metadata } from "next";
import { notFound } from "next/navigation";
import Link from "next/link";
import { MdOutlineUpdate } from "react-icons/md";
import { IoIosCheckmark } from "react-icons/io";
import { CiGlobe } from "react-icons/ci";
import { auth } from "@/libs/auth";
import { getCourseById } from "@/services/api/course";
import { Breadcrumbs, Rating } from "./components";
import Sidebar from "./Sidebar";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import CoursePurchaseSection from "./CoursePurchaseSection";

export async function generateMetadata({
    params,
}: {
    params: { id: string };
}): Promise<Metadata> {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    return {
        title: `${course.title} | Vicourses`,
        description: course.title,
        openGraph: {
            title: `${course.title} | Vicourses`,
            description: course.title,
        },
    };
}

export default async function CoursePage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    const updatedAt = new Date(course.updatedAt);

    return (
        <div className="relative">
            <div className="absolute right-0 top-10 hidden lg:block">
                <Sidebar course={course} />
            </div>

            <div className="py-5 w-full lg:max-w-[66.67%]">
                <div>
                    <Breadcrumbs className="bg-transparent p-0 py-3">
                        <Link
                            href={`/category/${course.category.slug}`}
                            className="text-[#5022cf]"
                        >
                            {course.category.name}
                        </Link>
                        <Link
                            href={`/category/${course.subCategory.slug}`}
                            className="text-[#5022cf]"
                        >
                            {course.subCategory.name}
                        </Link>
                    </Breadcrumbs>

                    <div
                        className="block lg:hidden w-full border border-gray-700 mb-5"
                        style={{
                            aspectRatio: "calc(1 / 0.5625)",
                        }}
                    >
                        <img
                            className="w-full h-full"
                            src={
                                course.thumbnailUrl ||
                                DEFAULT_COURSE_THUMBNAIL_URL
                            }
                            alt={course.title}
                        />
                    </div>

                    <h1 className="text-black text-2xl lg:text-3xl font-bold mb-5">
                        {course.title}
                    </h1>
                    <div className="flex items-center gap-2 font-bold text-blue-gray-500 mb-3">
                        {course.rating}
                        <Rating value={Math.floor(course.rating)} readonly />
                        <p
                            color="blue-gray"
                            className="font-medium text-blue-gray-500"
                        >
                            {course.studentCount} students
                        </p>
                    </div>
                    <div className="text-blue-gray-500 mb-2">
                        Created by{" "}
                        <Link
                            href={`/user/${course.user.id}`}
                            className="underline text-[#5022cf]"
                        >
                            {course.user.name}
                        </Link>
                    </div>
                    <div className="text-blue-gray-500 flex gap-2 items-center mb-2">
                        <div className="flex gap-2 items-center">
                            <MdOutlineUpdate size={14} /> Last updated{" "}
                            {`${
                                updatedAt.getMonth() + 1
                            }/${updatedAt.getFullYear()}`}
                        </div>
                        <div className="flex gap-2 items-center">
                            <CiGlobe size={14} />
                            {course.locale?.englishTitle}
                        </div>
                    </div>
                    <div>
                        <div>Tags:</div>
                        <div className="flex flex-wrap gap-3">
                            {course.tags.map((tag) => (
                                <div
                                    key={tag}
                                    className="rounded-full px-2 bg-[#daebfc]"
                                >
                                    {tag}
                                </div>
                            ))}
                        </div>
                    </div>
                </div>

                <div className="block lg:hidden">
                    <CoursePurchaseSection course={course} />
                </div>

                <div className="border border-gray-800 p-5 mb-5 mt-10">
                    <h2 className="text-black font-semibold text-2xl mb-5">
                        Contents
                    </h2>
                    <div className="grid gap-3 md:grid-cols-2 grid-cols-1">
                        {course.learnedContents?.map((content, index) => (
                            <div
                                key={index}
                                className="flex gap-2 items-center text-gray-900"
                            >
                                <IoIosCheckmark
                                    size={32}
                                    className="flex-shrink-0"
                                />
                                <p className="flex-grow">{content}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}
