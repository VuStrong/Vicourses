"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
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
import useCurriculum from "../_hooks/useCurriculum";

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
    initialLessonId,
}: {
    courseId: string;
    initialLessonId?: string;
}) {
    const isLoading = useCurriculum((state) => state.isLoadingCurriculum);
    const curriculum = useCurriculum((state) => state.curriculum);
    const currentLesson = useCurriculum((state) => state.currentLesson);
    const fetchCurriculum = useCurriculum((state) => state.fetchCurriculum);
    const setCurrentLesson = useCurriculum((state) => state.setCurrentLesson);

    const [openSections, setOpenSections] = useState<string[]>([]);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated") {
            fetchCurriculum(courseId, session.accessToken, initialLessonId);
        }
    }, [courseId, status]);

    useEffect(() => {
        if (currentLesson && curriculum) {
            const sectionToOpen = getSectionIdByLesson(
                curriculum,
                currentLesson.id
            );

            if (sectionToOpen) setOpenSections([sectionToOpen]);
        }
    }, [currentLesson]);

    const onClickSection = (sectionId: string) => {
        if (openSections.includes(sectionId)) {
            setOpenSections(openSections.filter((id) => id !== sectionId));
        } else {
            setOpenSections([...openSections, sectionId]);
        }
    };

    const onClickLesson = (lessonId: string) => {
        setCurrentLesson(lessonId);
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
                                            active={
                                                lesson.id === currentLesson?.id
                                            }
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
