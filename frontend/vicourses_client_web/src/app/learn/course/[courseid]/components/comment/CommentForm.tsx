"use client";

import { useState } from "react";
import dynamic from "next/dynamic";
import { Session } from "next-auth";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import { Avatar, Button, Typography } from "@material-tailwind/react";
import "react-quill/dist/quill.snow.css";

import { Lesson } from "@/libs/types/lesson";
import { LessonComment } from "@/libs/types/lesson-comment";
import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import { createLessonComment } from "@/services/api/lesson-comment";
import useUser from "@/hooks/useUser";

const ReactQuill = dynamic(() => import("react-quill"), { ssr: false });

export default function CommentForm({
    lessonId,
    replyTo,
    session,
    onCommentCreated,
}: {
    lessonId: string;
    replyTo?: LessonComment;
    session: Session | null;
    onCommentCreated: (comment: LessonComment) => void;
}) {
    const { user } = useUser("id,name,thumbnailUrl");
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const {
        handleSubmit,
        reset,
        control,
        formState: { isDirty },
    } = useForm<FieldValues>({
        defaultValues: {
            content: "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsSubmitting(true);

        try {
            const comment = await createLessonComment(
                lessonId,
                {
                    content: data.content,
                    replyToId: replyTo?.id,
                },
                session?.accessToken || ""
            );

            onCommentCreated(comment);
            reset();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsSubmitting(false);
    };

    return (
        <div className="flex gap-2">
            <div>
                <Avatar
                    src={user?.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                    alt={user?.name}
                />
            </div>
            <form onSubmit={handleSubmit(onSubmit)} className="flex-grow">
                <Controller
                    name="content"
                    control={control}
                    rules={{
                        required: {
                            value: true,
                            message: "Comment must not empty",
                        },
                    }}
                    render={({ field, fieldState }) => (
                        <>
                            <ReactQuill
                                theme="snow"
                                placeholder="Write comment"
                                value={field.value}
                                onChange={field.onChange}
                                modules={{
                                    toolbar: ["bold", "italic", "code"],
                                }}
                                className={`${
                                    !!fieldState.error && "border border-error"
                                }`}
                            />
                            {fieldState.error && (
                                <Typography
                                    variant="small"
                                    color="red"
                                    className="mt-2 flex items-center gap-1 font-normal"
                                >
                                    {fieldState.error.message}
                                </Typography>
                            )}
                        </>
                    )}
                />

                <div className="flex justify-end mt-3">
                    <Button
                        className="bg-primary w-full md:w-auto flex justify-center"
                        size="md"
                        type="submit"
                        disabled={!isDirty || isSubmitting}
                        loading={isSubmitting}
                        onClick={handleSubmit(onSubmit)}
                    >
                        Submit
                    </Button>
                </div>
            </form>
        </div>
    );
}
