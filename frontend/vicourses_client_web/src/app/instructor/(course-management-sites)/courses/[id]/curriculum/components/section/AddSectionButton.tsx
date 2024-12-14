"use client";

import { FaPlus } from "react-icons/fa6";
import { Button, Input, Typography } from "@material-tailwind/react";
import { Session } from "next-auth";
import { useState } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import toast from "react-hot-toast";

import { Section } from "@/libs/types/section";
import { createSection } from "@/services/api/course-section";

type FormValues = {
    title: string;
    description?: string;
};

const titleErrorMessage = "Title must be between 3 and 80 characters.";

export default function AddSectionButton({
    courseId,
    session,
    onSectionAdded,
}: {
    courseId: string;
    session: Session | null;
    onSectionAdded?: (section: Section) => void;
}) {
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [isCreating, setIsCreating] = useState<boolean>(false);

    const { handleSubmit, reset, control } = useForm<FormValues>({
        defaultValues: {
            title: "",
            description: "",
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        setIsCreating(true);

        try {
            const section = await createSection(
                {
                    title: data.title,
                    description: data.description,
                    courseId,
                },
                session?.accessToken || ""
            );

            toast.success("Section added");
            reset();
            setOpenForm(false);
            onSectionAdded?.(section);
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

    return (
        <>
            {openForm ? (
                <div className="border border-gray-700 p-2 flex gap-2 flex-wrap">
                    <div className="text-black font-semibold">New section</div>
                    <form className="flex-grow flex flex-col gap-5">
                        <Controller
                            name="title"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: titleErrorMessage,
                                },
                                minLength: {
                                    value: 3,
                                    message: titleErrorMessage,
                                },
                                maxLength: {
                                    value: 80,
                                    message: titleErrorMessage,
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
                                        What can students do after completing this
                                        section?
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
            ) : (
                <Button
                    onClick={() => setOpenForm(true)}
                    className="text-black bg-white rounded-none border border-gray-700 flex items-center gap-3"
                    size="sm"
                >
                    <FaPlus size={24} />
                    Section
                </Button>
            )}
        </>
    );
}
