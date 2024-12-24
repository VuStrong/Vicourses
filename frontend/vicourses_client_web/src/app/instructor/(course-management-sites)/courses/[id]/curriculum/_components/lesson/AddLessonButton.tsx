"use client";

import { useState } from "react";
import { Session } from "next-auth";
import { Button, Input, Typography } from "@material-tailwind/react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import { FaPlus } from "react-icons/fa";
import { IoCloseSharp } from "react-icons/io5";

import { Lesson, LessonType } from "@/libs/types/lesson";
import { SectionInInstructorCurriculum } from "@/libs/types/section";
import { createLesson } from "@/services/api/course-lesson";

type AddLessonFormValues = {
    title: string;
    description: string;
};

export default function AddLessonButton({
    onLessonAdded,
    section,
    session,
}: {
    onLessonAdded: (lesson: Lesson) => void;
    section: SectionInInstructorCurriculum;
    session: Session | null;
}) {
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [openSelectType, setOpenSelectType] = useState<boolean>(false);
    const [type, setType] = useState<LessonType>("Video");
    const [isCreating, setIsCreating] = useState<boolean>(false);

    const { handleSubmit, reset, control } = useForm<AddLessonFormValues>({
        defaultValues: {
            title: "",
            description: "",
        },
    });

    const onSubmit: SubmitHandler<AddLessonFormValues> = async (data) => {
        setIsCreating(true);

        try {
            const lesson = await createLesson(
                {
                    title: data.title,
                    description: data.description,
                    courseId: section.courseId,
                    sectionId: section.id,
                    type,
                },
                session?.accessToken || ""
            );

            toast.success("Lesson added");
            reset();
            setOpenForm(false);
            onLessonAdded?.(lesson);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsCreating(false);
    };

    const handleCloseForm = () => {
        if (!isCreating) {
            reset();
        }
        setOpenForm(false);
    };

    const handleSelectType = (type: LessonType) => {
        setType(type);
        setOpenSelectType(false);
        setOpenForm(true);
    };

    return (
        <>
            {openSelectType && (
                <div className="p-2 flex gap-2 flex-wrap items-center">
                    <button onClick={() => setOpenSelectType(false)}>
                        <IoCloseSharp size={24} />
                    </button>
                    <div className="border border-gray-700 px-2 py-1 flex gap-3 flex-wrap">
                        <Button
                            onClick={() => handleSelectType("Video")}
                            size="sm"
                            variant="text"
                            className="text-primary flex gap-2 flex-nowrap"
                        >
                            <FaPlus scale={24} />
                            Video
                        </Button>
                        <Button
                            onClick={() => handleSelectType("Quiz")}
                            size="sm"
                            variant="text"
                            className="text-primary flex gap-2 flex-nowrap"
                        >
                            <FaPlus scale={24} />
                            Quiz
                        </Button>
                    </div>
                </div>
            )}

            {openForm && (
                <div className="border border-gray-700 p-2 flex gap-2 flex-wrap">
                    <form className="flex-grow flex flex-col gap-5">
                        <Controller
                            name="title"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message:
                                        "Title must be between 3 and 80 characters.",
                                },
                                minLength: {
                                    value: 3,
                                    message:
                                        "Title must be between 3 and 80 characters.",
                                },
                                maxLength: {
                                    value: 80,
                                    message:
                                        "Title must be between 3 and 80 characters.",
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
                                    message:
                                        "Description must not greeter than 200",
                                },
                            }}
                            render={({ field, fieldState }) => (
                                <div>
                                    <label
                                        htmlFor="description"
                                        className="text-black font-semibold text-sm"
                                    >
                                        What can students do after completing
                                        this lesson?
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
                                onClick={handleCloseForm}
                                type="button"
                            >
                                Cancel
                            </Button>
                            <Button
                                className="bg-black text-white rounded-none"
                                size="sm"
                                onClick={handleSubmit(onSubmit)}
                                type="submit"
                                loading={isCreating}
                            >
                                Create
                            </Button>
                        </div>
                    </form>
                </div>
            )}
            {!openForm && !openSelectType && (
                <Button
                    onClick={() => setOpenSelectType(true)}
                    className="text-black bg-white rounded-none border border-gray-700 flex items-center gap-3"
                    size="sm"
                >
                    <FaPlus size={16} />
                    Lesson
                </Button>
            )}
        </>
    );
}
