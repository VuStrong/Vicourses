"use client";

import { useState } from "react";
import { Session } from "next-auth";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { IoMdMenu, IoMdTrash } from "react-icons/io";
import { IoPencil } from "react-icons/io5";
import {
    MdOutlineOndemandVideo,
    MdOutlineQuiz,
    MdOutlinePreview,
} from "react-icons/md";
import toast from "react-hot-toast";
import { Button, Input, Typography } from "@material-tailwind/react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";

import { Lesson, Quiz } from "@/libs/types/lesson";
import { VideoFile } from "@/libs/types/common";
import { updateLesson } from "@/services/api/course-lesson";
import LessonQuizzesContainer from "./LessonQuizzesContainer";
import LessonVideoUpload from "./LessonVideoUpload";
import useDeleteCurriculumItemModal from "../../_hooks/useDeleteCurriculumItemModal";

type ChangeableLessonStates = {
    title: string;
    description: string;
    video: VideoFile | null;
    quizzes: Quiz[];
};

export default function LessonItem({
    lesson,
    session,
}: {
    lesson: Lesson;
    session: Session | null;
}) {
    const [state, setState] = useState<ChangeableLessonStates>({
        title: lesson.title,
        description: lesson.description || "",
        video: lesson.video,
        quizzes: lesson.quizzes,
    });
    const [expanded, setExpanded] = useState<boolean>(false);
    const [openFormEdit, setOpenFormEdit] = useState<boolean>(false);
    const openModal = useDeleteCurriculumItemModal((state) => state.open);

    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({
            id: lesson.id,
            data: {
                type: "Lesson",
                sectionId: lesson.sectionId,
                title: state.title,
            },
        });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    return (
        <div
            ref={setNodeRef}
            style={style}
            className="bg-white border border-gray-700 py-5 px-2"
        >
            {openFormEdit ? (
                <FormEdit
                    lessonId={lesson.id}
                    initialState={state}
                    session={session}
                    close={() => setOpenFormEdit(false)}
                    onEdited={(editedState) => setState({ ...editedState })}
                />
            ) : (
                <div
                    className={`flex justify-between ${expanded ? "pb-5" : ""}`}
                >
                    <div className="flex flex-col md:flex-row gap-2 md:items-center">
                        <span className="text-black font-semibold">
                            {lesson.type}:{" "}
                        </span>

                        <div className="flex gap-2 flex-nowrap items-center">
                            {lesson.type === "Video" ? (
                                <MdOutlineOndemandVideo size={18} />
                            ) : (
                                <MdOutlineQuiz size={18} />
                            )}
                            <span className="text-gray-800">{state.title}</span>
                        </div>

                        <div className="flex gap-2">
                            <button
                                onClick={() => {
                                    setOpenFormEdit(true);
                                    setExpanded(false);
                                }}
                            >
                                <IoPencil size={18} />
                            </button>
                            <button
                                onClick={() =>
                                    openModal({
                                        id: lesson.id,
                                        type: "Lesson",
                                        sectionId: lesson.sectionId,
                                    })
                                }
                            >
                                <IoMdTrash size={18} />
                            </button>
                            <a
                                title="Preview"
                                target="_blank"
                                href={`/learn/course/${lesson.courseId}?lesson=${lesson.id}`}
                            >
                                <MdOutlinePreview size={18} />
                            </a>
                        </div>
                    </div>
                    <div className="flex gap-2 items-center">
                        <button
                            onClick={() => setExpanded(!expanded)}
                            className={expanded ? "rotate-180" : ""}
                        >
                            &#11206;
                        </button>
                        <div
                            {...listeners}
                            {...attributes}
                            className="cursor-move"
                        >
                            <IoMdMenu size={24} />
                        </div>
                    </div>
                </div>
            )}
            {expanded && (
                <div className="border-t border-gray-700 py-2">
                    {lesson.type === "Quiz" && (
                        <LessonQuizzesContainer
                            lessonId={lesson.id}
                            quizzes={state.quizzes}
                            session={session}
                            onQuizzesChanged={(lesson) => {
                                setState({
                                    ...state,
                                    quizzes: lesson.quizzes,
                                });
                            }}
                        />
                    )}
                    {lesson.type === "Video" && (
                        <LessonVideoUpload
                            lessonId={lesson.id}
                            video={state.video}
                            session={session}
                            onVideoChanged={(video) => {
                                setState({
                                    ...state,
                                    video,
                                });
                            }}
                        />
                    )}
                </div>
            )}
        </div>
    );
}

//
function FormEdit({
    lessonId,
    initialState,
    close,
    session,
    onEdited,
}: {
    lessonId: string;
    initialState: ChangeableLessonStates;
    close: () => void;
    session: Session | null;
    onEdited: (state: ChangeableLessonStates) => void;
}) {
    const [isEditing, setIsEditing] = useState<boolean>(false);

    const { handleSubmit, control, reset } = useForm<ChangeableLessonStates>({
        defaultValues: {
            title: initialState.title,
            description: initialState.description,
        },
    });

    const onSubmit: SubmitHandler<ChangeableLessonStates> = async (data) => {
        setIsEditing(true);

        try {
            await updateLesson(
                lessonId,
                {
                    title: data.title,
                    description: data.description,
                },
                session?.accessToken || ""
            );

            onEdited({ ...initialState, ...data });
            reset(data);
            close();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsEditing(false);
    };

    const handleClose = () => {
        if (!isEditing) {
            reset();
        }
        close();
    };

    return (
        <form className="flex-grow flex flex-col gap-5">
            <Controller
                name="title"
                control={control}
                rules={{
                    required: {
                        value: true,
                        message: "Title must be between 3 and 80 characters.",
                    },
                    minLength: {
                        value: 3,
                        message: "Title must be between 3 and 80 characters.",
                    },
                    maxLength: {
                        value: 80,
                        message: "Title must be between 3 and 80 characters.",
                    },
                }}
                render={({ field, fieldState }) => (
                    <div>
                        <Input
                            label="Title"
                            {...field}
                            crossOrigin={undefined}
                            error={!!fieldState.error}
                        />
                        <div></div>
                        {fieldState.error && (
                            <Typography
                                variant="small"
                                color="red"
                                className="mt-2 flex items-center gap-1 font-normal"
                            >
                                {fieldState.error.message}
                            </Typography>
                        )}
                    </div>
                )}
            />
            <Controller
                name="description"
                control={control}
                rules={{
                    maxLength: {
                        value: 200,
                        message: "Description must not greeter than 200",
                    },
                }}
                render={({ field, fieldState }) => (
                    <div>
                        <label
                            htmlFor="description"
                            className="text-black font-semibold text-sm"
                        >
                            What can students do after completing this lesson?
                        </label>
                        <Input
                            id="description"
                            label="Description"
                            {...field}
                            crossOrigin={undefined}
                            error={!!fieldState.error}
                        />
                        <div></div>
                        {fieldState.error && (
                            <Typography
                                variant="small"
                                color="red"
                                className="mt-2 flex items-center gap-1 font-normal"
                            >
                                {fieldState.error.message}
                            </Typography>
                        )}
                    </div>
                )}
            />
            <div className="flex gap-3 justify-end">
                <Button
                    variant="text"
                    size="sm"
                    onClick={handleClose}
                    type="button"
                >
                    Cancel
                </Button>
                <Button
                    className="bg-black text-white rounded-none"
                    size="sm"
                    onClick={handleSubmit(onSubmit)}
                    type="submit"
                    loading={isEditing}
                >
                    Save
                </Button>
            </div>
        </form>
    );
}
