"use client";

import { MdOutlineOndemandVideo } from "react-icons/md";
import { MdOutlineQuiz } from "react-icons/md";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { Loader } from "@/components/common";
import { PublicCurriculum } from "@/libs/types/course";
import { getPublicCurriculum } from "@/services/api/course";
import { formatLength } from "@/libs/utils";

export default function PublicCurriculumSection({
    courseId,
}: {
    courseId: string;
}) {
    const [openSections, setOpenSections] = useState<string[]>([]);
    const [curriculum, setCurriculum] = useState<PublicCurriculum | null>(null);
    const { data: session, status } = useSession();

    const onClickSection = (sectionId: string) => {
        if (openSections.includes(sectionId)) {
            setOpenSections(openSections.filter((id) => id !== sectionId));
        } else {
            setOpenSections([...openSections, sectionId]);
        }
    };

    useEffect(() => {
        if (status === "loading") return;

        (async () => {
            const result = await getPublicCurriculum(
                courseId,
                session?.accessToken
            );

            setCurriculum(result);

            if (result?.sections[0]) {
                setOpenSections([result.sections[0].id]);
            }
        })();
    }, [courseId, status]);

    return (
        <section className="mt-10">
            <h2 className="text-black font-semibold text-2xl mb-5">
                Curriculum
            </h2>

            {!curriculum ? (
                <div className="flex justify-center">
                    <Loader />
                </div>
            ) : (
                <div>
                    <div className="mb-2 text-gray-800">
                        {curriculum.totalSection} sections -{" "}
                        {curriculum.totalLesson} lessons -{" "}
                        {formatLength(curriculum.totalDuration)} duration
                    </div>
                    {curriculum.sections.map((section, index) => (
                        <Accordion
                            key={section.id}
                            title={`Section ${index + 1}: ${section.title}`}
                            open={openSections.includes(section.id)}
                            onClick={() => onClickSection(section.id)}
                            info={
                                <div>
                                    {section.lessonCount} lessons -{" "}
                                    {formatLength(section.duration)}
                                </div>
                            }
                        >
                            <div className="flex flex-col gap-3 text-gray-800">
                                {section.lessons.map((lesson) => (
                                    <div
                                        key={lesson.id}
                                        className="flex gap-3 items-center"
                                    >
                                        {lesson.type === "Video" ? (
                                            <MdOutlineOndemandVideo />
                                        ) : (
                                            <MdOutlineQuiz />
                                        )}
                                        <div className="flex-grow line-clamp-1">
                                            {lesson.title}
                                        </div>
                                        <div className="hidden md:block">
                                            {lesson.type === "Video"
                                                ? formatLength(lesson.duration)
                                                : `${lesson.quizzesCount} quizzes`}
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </Accordion>
                    ))}
                </div>
            )}
        </section>
    );
}

function Accordion({
    title,
    open,
    onClick,
    info,
    children,
}: {
    title: string;
    open: boolean;
    onClick: () => void;
    info: React.ReactNode;
    children: React.ReactNode;
}) {
    return (
        <>
            <div
                onClick={onClick}
                className={`border-x border-t last:border-b border-gray-500 px-5 py-3 flex items-center justify-between bg-[#f7f9fa] cursor-pointer`}
            >
                <div className="flex gap-3">
                    {open ? <div>&#11205;</div> : <div>&#11206;</div>}
                    <div className="text-black font-bold line-clamp-1">
                        {title}
                    </div>
                </div>
                <div className="hidden md:block whitespace-nowrap">{info}</div>
            </div>
            {open && (
                <div className="bg-white border-x border-t last:border-b border-gray-500 p-5">
                    {children}
                </div>
            )}
        </>
    );
}
