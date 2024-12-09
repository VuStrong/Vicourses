import { Metadata } from "next";
import { notFound, redirect } from "next/navigation";
import { auth } from "@/libs/auth";
import { getCourseById } from "@/services/api/course";
import CourseManagementHeader from "./components/CourseManagementHeader";
import CourseManagementSidebar from "./components/CourseManagementSidebar";
import OpenDrawerButton from "./components/OpenDrawerButton";

export const metadata: Metadata = {
    title: "Manage course | Vicourses",
    openGraph: {
        title: "Manage course | Vicourses",
    },
};

export default async function CourseManagementPagesLayout({
    children,
    params,
}: Readonly<{
    children: React.ReactNode;
    params: { id: string };
}>) {
    const session = await auth();
    const course = session
        ? await getCourseById(params.id, session.accessToken)
        : null;

    if (!course) notFound();

    if (session?.user.id !== course.user.id) redirect("/");

    return (
        <>
            <CourseManagementHeader course={course} />

            <div className="lg:flex py-20 px-5 lg:px-20">
                <div className="hidden lg:block">
                    <CourseManagementSidebar courseId={course.id} />
                </div>
                <div className="lg:hidden">
                    <OpenDrawerButton courseId={course.id} />
                </div>
                <div className="flex-grow z-[5] bg-white">
                    <div className="shadow-2xl w-full min-h-screen p-5 lg:p-10">
                        {children}
                    </div>
                </div>
            </div>
        </>
    );
}
