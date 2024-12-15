"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";

import { CourseDetail } from "@/libs/types/course";
import { Lesson } from "@/libs/types/lesson";
import { getLesson } from "@/services/api/course-lesson";
import { Loader } from "@/components/common";
import LessonVideo from "./LessonVideo";
import LessonQuizzes from "./LessonQuizzes";

export default function LearnView({
    course,
    lessonId,
}: {
    course: CourseDetail;
    lessonId?: string;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [lesson, setLesson] = useState<Lesson | null>(null);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated" && lessonId) {
            (async () => {
                setIsLoading(true);

                const result = await getLesson(lessonId, session.accessToken);

                setLesson(result);
                setIsLoading(false);
            })();
        }
    }, [lessonId, status]);

    if (isLoading) {
        return (
            <div className="flex justify-center items-center">
                <Loader />
            </div>
        );
    }

    if (!lesson) {
        return <div className="flex justify-center">No lesson found</div>;
    }

    return (
        <div>
            {lesson.type === "Video" && (
                <LessonVideo lesson={lesson} />
            )}
            {lesson.type === "Quiz" && (
                <LessonQuizzes lesson={lesson} />
            )}

            <div className="p-10">
                <h1 className="text-black font-bold text-2xl mb-3">{lesson.title}</h1>
                <p className="text-black">{lesson.description}</p>
            </div>
        </div>
    );
}
