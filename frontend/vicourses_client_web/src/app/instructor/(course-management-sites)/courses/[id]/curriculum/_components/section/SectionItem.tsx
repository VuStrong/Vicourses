"use client";

import { useState } from "react";
import { Session } from "next-auth";
import toast from "react-hot-toast";
import {
    SortableContext,
    useSortable,
    verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { Button, Input, Typography } from "@material-tailwind/react";
import { IoMdMenu, IoMdTrash } from "react-icons/io";
import { IoPencil } from "react-icons/io5";
import { Controller, SubmitHandler, useForm } from "react-hook-form";

import { SectionInInstructorCurriculum } from "@/libs/types/section";
import { Lesson } from "@/libs/types/lesson";
import { updateSection } from "@/services/api/course-section";
import LessonItem from "../lesson/LessonItem";
import AddLessonButton from "../lesson/AddLessonButton";
import useDeleteCurriculumItemModal from "../../_hooks/useDeleteCurriculumItemModal";

type ChangeableSectionStates = {
    title: string;
    description: string;
};

export default function SectionItem({
    section,
    index,
    session,
    onLessonAdded,
}: {
    section: SectionInInstructorCurriculum;
    index: number;
    session: Session | null;
    onLessonAdded: (lesson: Lesson) => void,
}) {
    const [state, setState] = useState<ChangeableSectionStates>({
        title: section.title,
        description: section.description || "",
    });
    const openModal = useDeleteCurriculumItemModal((state) => state.open);
    const [expanded, setExpanded] = useState<boolean>(false);
    const [openFormEdit, setOpenFormEdit] = useState<boolean>(false);

    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({
            id: section.id,
            data: {
                type: "Section",
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
            className="bg-[#f7f9fa] border border-gray-700 py-5 px-2"
        >
            {openFormEdit ? (
                <FormEdit
                    sectionId={section.id}
                    initialState={state}
                    session={session}
                    close={() => setOpenFormEdit(false)}
                    onEdited={(state) => setState({ ...state })}
                />
            ) : (
                <div className="flex justify-between">
                    <div className="flex flex-col md:flex-row gap-2 items-center">
                        <span className="text-black font-semibold">
                            Section {index}:{" "}
                        </span>

                        <span className="text-gray-800">{state.title}</span>

                        <div className="flex gap-2">
                            <button onClick={() => setOpenFormEdit(true)}>
                                <IoPencil size={18} />
                            </button>
                            <button
                                onClick={() =>
                                    openModal({
                                        id: section.id,
                                        type: "Section",
                                    })
                                }
                            >
                                <IoMdTrash size={18} />
                            </button>
                        </div>
                    </div>
                    <div className="flex gap-2 items-center">
                        <button
                            onClick={() => setExpanded(!expanded)}
                            className={expanded ? "rotate-180" : ""}
                        >
                            &#11206;
                        </button>
                        <div {...listeners} {...attributes} className="cursor-move">
                            <IoMdMenu size={24} />
                        </div>
                    </div>
                </div>
            )}
            <div className={`pt-8 pl-0 md:pl-8 ${expanded ? 'flex' : 'hidden'} flex-col gap-5 min-h-[65px]`}>
                <SortableContext
                    items={section.lessons}
                    strategy={verticalListSortingStrategy}
                >
                    {section.lessons.map((lesson) => (
                        <LessonItem
                            key={lesson.id}
                            lesson={lesson}
                            session={session}
                        />
                    ))}
                </SortableContext>

                <div>
                    <AddLessonButton
                        section={section}
                        session={session}
                        onLessonAdded={onLessonAdded}
                    />
                </div>
            </div>
        </div>
    );
}

//
function FormEdit({
    sectionId,
    initialState,
    close,
    session,
    onEdited,
}: {
    sectionId: string;
    initialState: ChangeableSectionStates;
    close: () => void;
    session: Session | null;
    onEdited: (state: ChangeableSectionStates) => void;
}) {
    const [isEditing, setIsEditing] = useState<boolean>(false);

    const { handleSubmit, control, reset } = useForm<ChangeableSectionStates>({
        defaultValues: {
            title: initialState.title,
            description: initialState.description,
        },
    });

    const onSubmit: SubmitHandler<ChangeableSectionStates> = async (data) => {
        setIsEditing(true);

        try {
            await updateSection(
                sectionId,
                {
                    title: data.title,
                    description: data.description,
                },
                session?.accessToken || ""
            );

            onEdited(data);
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
                            What can students do after completing this section?
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
