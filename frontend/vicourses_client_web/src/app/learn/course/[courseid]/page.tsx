import { Metadata } from "next";
import { notFound, redirect } from "next/navigation";

import { auth } from "@/libs/auth";
import { checkEnroll, getCourseById } from "@/services/api/course";
import BottomNav from "./_components/BottomNav";
import LearnView from "./_components/LearnView";
import Sidebar from "./_components/Sidebar";
import Header from "./_components/Header";
import { CurriculumStoreProvider } from "./_hooks/useCurriculum";

export async function generateMetadata({
    params,
}: {
    params: { courseid: string };
}): Promise<Metadata> {
    const session = await auth();
    const course = await getCourseById(params.courseid, session?.accessToken);

    if (!course) notFound();

    return {
        title: `Learn ${course.title} | Vicourses`,
    };
}

export default async function CourseLearnPage({
    params,
    searchParams,
}: Readonly<{
    params: { courseid: string };
    searchParams?: { [key: string]: string | string[] | undefined };
}>) {
    const session = await auth();
    if (!session) redirect("/");

    const course = await getCourseById(params.courseid, session.accessToken);

    if (!course) notFound();

    if (session.user.id !== course.user.id) {
        const enrolled = await checkEnroll(course.id, session.accessToken);

        if (!enrolled) {
            redirect("/");
        }
    }

    const lessonId = searchParams?.lesson
        ? Array.isArray(searchParams.lesson)
            ? searchParams.lesson[0]
            : searchParams.lesson
        : undefined;

    return (
        <CurriculumStoreProvider>
            <Header course={course} />
            <Sidebar courseId={course.id} lessonId={lessonId} />
            <BottomNav />
            <main className="w-full lg:w-[calc(100%-24rem)]">
                <LearnView />
            </main>
        </CurriculumStoreProvider>
    );
}
