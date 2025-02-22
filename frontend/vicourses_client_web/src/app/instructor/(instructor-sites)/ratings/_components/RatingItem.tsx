"use client";

import { useState } from "react";
import { useSession } from "next-auth/react";
import {
    Avatar,
    Button,
    Rating as RatingComponent,
    Textarea,
    Typography,
} from "@material-tailwind/react";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";

import { DEFAULT_COURSE_THUMBNAIL_URL, DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import { Rating } from "@/libs/types/rating";
import { respondRating } from "@/services/api/rating";

export default function RatingItem({
    initialRating,
}: {
    initialRating: Rating;
}) {
    const [rating, setRating] = useState<Rating>(initialRating);
    const [open, setOpen] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const { data: session } = useSession();

    const { handleSubmit, control, reset } = useForm<FieldValues>({
        defaultValues: {
            response: initialRating.response || "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsSubmitting(true);

        try {
            await respondRating(
                rating.id,
                data.response,
                session?.accessToken || ""
            );

            setRating({
                ...rating,
                responded: true,
                respondedAt: new Date().toISOString(),
                response: data.response,
            });
            reset(data);
            setOpen(false);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsSubmitting(false);
    };

    const handleCloseForm = () => {
        if (!isSubmitting) {
            reset();
        }
        setOpen(false);
    };

    return (
        <div className="border border-gray-700 p-5">
            {rating.course && (
                <div className="flex gap-3 mb-5 pb-2 border-b border-gray-600">
                    <img
                        className="flex-[1/5] max-w-[100px] object-cover aspect-video border border-gray-700 flex-shrink-0"
                        src={
                            rating.course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL
                        }
                        alt={rating.course.title}
                    />
                    <div>
                        <div className="text-black font-semibold line-clamp-2">
                            {rating.course.title}
                        </div>
                        <div className="text-gray-800">
                            Rating: {rating.course.avgRating}
                        </div>
                    </div>
                </div>
            )}

            <div className="flex gap-3">
                <Avatar
                    src={rating.user.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                    alt={rating.user.name}
                />
                <div>
                    <div className="text-black font-semibold line-clamp-1">
                        {rating.user.name}
                    </div>
                    <div>{new Date(rating.createdAt).toLocaleDateString()}</div>
                </div>
            </div>

            <div className="my-3">
                <RatingComponent value={rating.star} readonly />
            </div>

            <div className="text-gray-800 mb-5">{rating.feedback}</div>

            {open && (
                <div>
                    <Controller
                        name="response"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter response.",
                            },
                            maxLength: {
                                value: 255,
                                message: "Exceed maximum 255 characters",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <Textarea
                                    label="Write response"
                                    {...field}
                                    error={!!fieldState.error}
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
                            </div>
                        )}
                    />

                    <div className="flex gap-3">
                        <Button
                            onClick={handleCloseForm}
                            size="sm"
                            className="bg-white text-gray-700 border border-gray-700"
                        >
                            Cancel
                        </Button>
                        <Button
                            onClick={handleSubmit(onSubmit)}
                            size="sm"
                            className="bg-primary"
                            loading={isSubmitting}
                        >
                            Submit
                        </Button>
                    </div>
                </div>
            )}

            {rating.responded && !open && (
                <div className="border-l-2 border-gray-800 px-5">
                    {rating.respondedAt && (
                        <div className="text-black font-semibold">
                            Responded at{" "}
                            {new Date(rating.respondedAt).toLocaleDateString()}
                        </div>
                    )}
                    <div className="text-gray-800 mb-5">{rating.response}</div>
                    <Button
                        onClick={() => setOpen(true)}
                        size="sm"
                        className="bg-white text-gray-700 border border-gray-700"
                    >
                        Edit
                    </Button>
                </div>
            )}

            {!rating.responded && !open && (
                <Button
                    onClick={() => setOpen(true)}
                    size="sm"
                    className="bg-white text-gray-700 border border-gray-700"
                >
                    Respond
                </Button>
            )}
        </div>
    );
}
