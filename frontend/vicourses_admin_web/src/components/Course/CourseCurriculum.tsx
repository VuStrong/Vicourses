import { useEffect, useState } from "react";
import { MdOutlineOndemandVideo, MdOutlineQuiz } from "react-icons/md";
import { IoEyeOutline } from "react-icons/io5";
import { MediaPlayer, MediaProvider } from "@vidstack/react";
import {
    defaultLayoutIcons,
    DefaultVideoLayout,
} from "@vidstack/react/player/layouts/default";
import "@vidstack/react/player/styles/default/theme.css";
import "@vidstack/react/player/styles/default/layouts/video.css";

import { InstructorCurriculum, Lesson } from "../../types/course";
import axiosInstance from "../../libs/axios";
import { formatLength } from "../../libs/utils";
import Loader from "../Loader";
import Accordion from "../Accordion";
import { Modal, ModalBody, ModalHeader } from "../Modal";
import LessonQuizzes from "./LessonQuizzes";

export default function CourseCurriculum({ courseId }: { courseId: string }) {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [curriculum, setCurriculum] = useState<InstructorCurriculum>();
    const [selectedLesson, setSelectedLesson] = useState<Lesson>();

    useEffect(() => {
        setIsLoading(true);
        setCurriculum(undefined);
        setSelectedLesson(undefined);

        axiosInstance
            .get<InstructorCurriculum>(
                `/api/cs/v1/courses/${courseId}/instructor-curriculum`,
            )
            .then((response) => {
                setCurriculum(response.data);
            })
            .finally(() => setIsLoading(false));
    }, [courseId]);

    return (
        <>
            <div>
                {isLoading && (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                )}

                {!isLoading &&
                    (!curriculum || curriculum.sections.length === 0) && (
                        <div className="text-center">No result</div>
                    )}

                {!isLoading && curriculum && curriculum.sections.length > 0 && (
                    <>
                        {curriculum.sections.map((section, index) => (
                            <Accordion
                                key={section.id}
                                title={`Section ${index + 1}: ${section.title}`}
                                info={
                                    <div>
                                        {section.lessonCount} lessons -{" "}
                                        {formatLength(section.duration)}
                                    </div>
                                }
                            >
                                <div className="flex flex-col gap-3">
                                    {section.lessons.map((lesson) => (
                                        <LessonItem
                                            key={lesson.id}
                                            lesson={lesson}
                                            onView={() => {
                                                setSelectedLesson(lesson);
                                            }}
                                        />
                                    ))}
                                </div>
                            </Accordion>
                        ))}
                    </>
                )}
            </div>
            <Modal
                open={!!selectedLesson}
                onClose={() => setSelectedLesson(undefined)}
            >
                <ModalHeader className="flex justify-between flex-nowrap">
                    {selectedLesson?.title}
                    <button
                        onClick={() => setSelectedLesson(undefined)}
                        className="font-semibold"
                    >
                        &#10539;
                    </button>
                </ModalHeader>
                <ModalBody>
                    {selectedLesson?.type === "Video" && (
                        <div>
                            {selectedLesson.video?.status === "Processed" ? (
                                <MediaPlayer
                                    autoPlay={false}
                                    title={selectedLesson.title}
                                    src={
                                        selectedLesson.video?.streamFileUrl ||
                                        ""
                                    }
                                    className="w-full object-cover aspect-video"
                                >
                                    <MediaProvider />
                                    <DefaultVideoLayout
                                        thumbnails="https://files.vidstack.io/sprite-fight/thumbnails.vtt"
                                        icons={defaultLayoutIcons}
                                    />
                                </MediaPlayer>
                            ) : (
                                <div>This lesson doesn't have a video</div>
                            )}
                        </div>
                    )}
                    {selectedLesson?.type === "Quiz" && (
                        <LessonQuizzes lesson={selectedLesson} />
                    )}
                </ModalBody>
            </Modal>
        </>
    );
}

function LessonItem({
    lesson,
    onView,
}: {
    lesson: Lesson;
    onView: () => void;
}) {
    return (
        <div className="flex gap-3 items-center">
            <div className="flex-shrink-0">
                {lesson.type === "Video" ? (
                    <MdOutlineOndemandVideo />
                ) : (
                    <MdOutlineQuiz />
                )}
            </div>
            <div className="flex-grow flex gap-2 items-center">
                <div className="line-clamp-1">{lesson.title}</div>
                <button
                    className="hover:text-primary"
                    title="View"
                    onClick={onView}
                >
                    <IoEyeOutline />
                </button>
            </div>
            <div className="hidden md:block">
                {lesson.type === "Video"
                    ? formatLength(lesson.video?.duration || 0)
                    : `${lesson.quizzesCount} quizzes`}
            </div>
        </div>
    );
}
