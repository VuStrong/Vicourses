"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import {
    Accordion,
    AccordionBody,
    AccordionHeader,
} from "@material-tailwind/react";
import { MdOutlineOndemandVideo, MdOutlineQuiz } from "react-icons/md";

import { Loader } from "@/components/common";
import { PublicCurriculum } from "@/libs/types/course";
import { LessonType } from "@/libs/types/lesson";
import { formatLength } from "@/libs/utils";
import { getPublicCurriculum } from "@/services/api/course";
import useLessonNavigation from "../hooks/useLessonNavigation";

function getSectionIdByLesson(curriculum: PublicCurriculum, lessonId: string) {
    for (const section of curriculum.sections) {
        for (const lesson of section.lessons) {
            if (lesson.id === lessonId) {
                return section.id;
            }
        }
    }

    return null;
}

export default function Curriculum({
    courseId,
    lessonId,
}: {
    courseId: string;
    lessonId?: string;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [curriculum, setCurriculum] = useState<PublicCurriculum | null>(null);
    const [openSections, setOpenSections] = useState<string[]>([]);
    const router = useRouter();
    const { data: session, status } = useSession();

    const setLessonIds = useLessonNavigation(
        (state) => state.setIdsFromPublicCurriculum
    );
    const setCurrentLessonId = useLessonNavigation(
        (state) => state.setCurrentId
    );

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getPublicCurriculum(
                    courseId,
                    session.accessToken
                );

                if (result) {
                    setCurriculum(result);
                    setIsLoading(false);
                    setLessonIds(result, lessonId);

                    if (lessonId) {
                        const sectionToOpen = getSectionIdByLesson(
                            result,
                            lessonId
                        );

                        if (sectionToOpen) setOpenSections([sectionToOpen]);

                        return;
                    }

                    const lessonToRedirect = result.sections[0]?.lessons[0];

                    if (!lessonToRedirect) return;

                    router.push(
                        `/learn/course/${courseId}?lesson=${lessonToRedirect.id}`
                    );
                }
            })();
        }
    }, [courseId, status]);

    useEffect(() => {
        if (lessonId && curriculum) {
            setCurrentLessonId(lessonId);

            const sectionToOpen = getSectionIdByLesson(curriculum, lessonId);

            if (sectionToOpen) setOpenSections([sectionToOpen]);
        }
    }, [lessonId]);

    const onClickSection = (sectionId: string) => {
        if (openSections.includes(sectionId)) {
            setOpenSections(openSections.filter((id) => id !== sectionId));
        } else {
            setOpenSections([...openSections, sectionId]);
        }
    };

    const onClickLesson = (lessonId: string) => {
        router.push(`/learn/course/${courseId}?lesson=${lessonId}`);
    };

    return (
        <div>
            <div className="text-black font-semibold p-5 border-b border-gray-300">
                Curriculum
            </div>

            {isLoading && (
                <div className="flex justify-center">
                    <Loader />
                </div>
            )}

            {!isLoading &&
                curriculum?.sections.map((section, index) => {
                    const open = openSections.includes(section.id);

                    return (
                        <Accordion
                            key={section.id}
                            open={open}
                            icon={
                                open ? <div>&#11205;</div> : <div>&#11206;</div>
                            }
                        >
                            <AccordionHeader
                                onClick={() => onClickSection(section.id)}
                                className="text-base text-black bg-[#f7f9fa] p-5 items-start"
                            >
                                <div>
                                    Section {index + 1}: {section.title}
                                    <span className="block text-sm text-gray-600">
                                        {section.lessonCount} lesson |{" "}
                                        {formatLength(section.duration)}
                                    </span>
                                </div>
                            </AccordionHeader>
                            <AccordionBody className="border-b border-gray-300 py-0">
                                <div className="flex flex-col">
                                    {section.lessons.map((lesson) => (
                                        <LessonItem
                                            key={lesson.id}
                                            lesson={lesson}
                                            active={lesson.id === lessonId}
                                            onClick={() =>
                                                onClickLesson(lesson.id)
                                            }
                                        />
                                    ))}
                                </div>
                            </AccordionBody>
                        </Accordion>
                    );
                })}
        </div>
    );
}

function LessonItem({
    lesson,
    active,
    onClick,
}: {
    lesson: {
        id: string;
        title: string;
        order: number;
        type: LessonType;
        duration: number;
        quizzesCount: number;
    };
    active: boolean;
    onClick: () => void;
}) {
    return (
        <div
            className={`hover:bg-gray-300 cursor-pointer px-5 py-2 ${
                active && "bg-gray-300"
            }`}
            onClick={onClick}
        >
            <div className="text-black text-sm">
                {lesson.order}. {lesson.title}
            </div>
            <div className="flex gap-2 items-center">
                {lesson.type === "Video" ? (
                    <>
                        <MdOutlineOndemandVideo />
                        <div>{formatLength(lesson.duration)}</div>
                    </>
                ) : (
                    <>
                        <MdOutlineQuiz />
                        <div>{lesson.quizzesCount} quizzes</div>
                    </>
                )}
            </div>
        </div>
    );
}
