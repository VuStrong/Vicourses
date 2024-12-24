"use client";

import { useState } from "react";
import { Session } from "next-auth";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import {
    Button,
    Textarea,
    Typography,
    Rating as RatingComponent,
} from "@material-tailwind/react";
import toast from "react-hot-toast";

import { Rating } from "@/libs/types/rating";
import { createRating, updateRating } from "@/services/api/rating";

type FormValues = {
    star: number;
    feedback: string;
};

export default function RatingForm({
    initialRating,
    courseId,
    onCancel,
    session,
    onRatingChanged,
}: {
    initialRating: Rating | null;
    courseId: string;
    onCancel: () => void;
    session: Session | null;
    onRatingChanged: (rating: Rating) => void;
}) {
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const {
        handleSubmit,
        control,
        reset,
        formState: { isDirty },
    } = useForm<FormValues>({
        defaultValues: {
            star: initialRating?.star || 5,
            feedback: initialRating?.feedback || "",
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        if (!session || isSubmitting) return;

        setIsSubmitting(true);

        try {
            const rating = initialRating
                ? await updateRating(
                      initialRating.id,
                      data,
                      session.accessToken
                  )
                : await createRating(
                      {
                          courseId,
                          ...data,
                      },
                      session.accessToken
                  );

            onRatingChanged(rating);
            
            if (initialRating) {
                reset(data);
            } else {
                reset();
            }
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsSubmitting(false);
    };

    const handleCancel = () => {
        if (!isSubmitting) {
            reset();
        }
        onCancel();
    };

    return (
        <form>
            <Controller
                name="star"
                control={control}
                render={({ field }) => (
                    <RatingComponent
                        value={field.value}
                        onChange={field.onChange}
                        readonly={isSubmitting}
                    />
                )}
            />
            <Controller
                name="feedback"
                control={control}
                rules={{
                    required: {
                        value: true,
                        message: "Write your feedback",
                    },
                    maxLength: {
                        value: 400,
                        message: "Exceed maximum of 400 characters",
                    },
                }}
                render={({ field, fieldState }) => (
                    <>
                        <Textarea
                            disabled={isSubmitting}
                            {...field}
                            placeholder="Write your feedback"
                            error={!!fieldState.error}
                        />
                        {fieldState.error && (
                            <Typography
                                variant="small"
                                color="red"
                                className="font-normal"
                            >
                                {fieldState.error.message}
                            </Typography>
                        )}
                    </>
                )}
            />
            <div className="flex gap-3 flex-wrap mt-3">
                <Button
                    variant="text"
                    size="sm"
                    type="button"
                    className="rounded-none"
                    disabled={isSubmitting}
                    onClick={handleCancel}
                >
                    Cancel
                </Button>
                <Button
                    className="bg-primary rounded-none"
                    loading={isSubmitting}
                    size="sm"
                    type="submit"
                    disabled={!isDirty || isSubmitting}
                    onClick={handleSubmit(onSubmit)}
                >
                    Submit
                </Button>
            </div>
        </form>
    );
}
