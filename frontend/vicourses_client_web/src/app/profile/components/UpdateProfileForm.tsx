"use client";

import { Button, Input, Textarea, Typography } from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import { Loader } from "@/components/common";
import { User } from "@/libs/types/user";
import { getAuthenticatedUser, updateProfile } from "@/services/api/user";

export default function UpdateProfileForm() {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [user, setUser] = useState<User | null>(null);
    const urlRegex =
        /(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g;
    const { data: session } = useSession();

    const {
        handleSubmit,
        setValue,
        control,
        formState: { errors },
    } = useForm<FieldValues>();

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsUpdating(true);

        try {
            await updateProfile(
                {
                    name: data.name,
                    headline: data.headline || undefined,
                    description: data.description || undefined,
                    websiteUrl: data.websiteUrl || undefined,
                    youtubeUrl: data.youtubeUrl || undefined,
                    facebookUrl: data.facebookUrl || undefined,
                    linkedInUrl: data.linkedInUrl || undefined,
                },
                session?.accessToken || ""
            );

            toast.success("Profile saved");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    useEffect(() => {
        (async () => {
            if (session?.accessToken) {
                const result = await getAuthenticatedUser(session.accessToken);

                setUser(result);

                if (result) {
                    setValue("name", result.name);
                    setValue("headline", result.headline || "");
                    setValue("description", result.description || "");
                    setValue("websiteUrl", result.websiteUrl || "");
                    setValue("youtubeUrl", result.youtubeUrl || "");
                    setValue("facebookUrl", result.facebookUrl || "");
                    setValue("linkedInUrl", result.linkedInUrl || "");
                }
            }
        })();
    }, [session?.accessToken]);

    if (!user) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <form className="my-5 w-full">
            <div className="flex flex-wrap gap-3">
                <div className="flex-1 flex flex-col gap-5">
                    <Controller
                        name="name"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter your name.",
                            },
                            minLength: {
                                value: 2,
                                message:
                                    "Name must be between 2 and 50 characters",
                            },
                            maxLength: {
                                value: 50,
                                message:
                                    "Name must be between 2 and 50 characters",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Name"
                                    crossOrigin={undefined}
                                    error={!!errors[field.name]}
                                />
                                {errors[field.name] && (
                                    <Typography
                                        variant="small"
                                        color="red"
                                        className="mt-2 flex items-center gap-1 font-normal"
                                    >
                                        {errors[
                                            field.name
                                        ]?.message?.toString()}
                                    </Typography>
                                )}
                            </div>
                        )}
                    />
                    <Controller
                        name="headline"
                        control={control}
                        rules={{
                            maxLength: {
                                value: 60,
                                message:
                                    "Headline must not contains more than 60 characters",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Headline"
                                    placeholder="Instructor at Vicourses"
                                    crossOrigin={undefined}
                                    error={!!errors[field.name]}
                                />
                                {errors[field.name] && (
                                    <Typography
                                        variant="small"
                                        color="red"
                                        className="mt-2 flex items-center gap-1 font-normal"
                                    >
                                        {errors[
                                            field.name
                                        ]?.message?.toString()}
                                    </Typography>
                                )}
                            </div>
                        )}
                    />
                    <Controller
                        name="description"
                        control={control}
                        rules={{}}
                        render={({ field }) => (
                            <div>
                                <Textarea {...field} label="Bio" />
                                <Typography
                                    variant="small"
                                    color="gray"
                                    className="mt-2 flex items-center gap-1 font-normal"
                                >
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        viewBox="0 0 24 24"
                                        fill="currentColor"
                                        className="-mt-px h-4 w-4"
                                    >
                                        <path
                                            fillRule="evenodd"
                                            d="M2.25 12c0-5.385 4.365-9.75 9.75-9.75s9.75 4.365 9.75 9.75-4.365 9.75-9.75 9.75S2.25 17.385 2.25 12zm8.706-1.442c1.146-.573 2.437.463 2.126 1.706l-.709 2.836.042-.02a.75.75 0 01.67 1.34l-.04.022c-1.147.573-2.438-.463-2.127-1.706l.71-2.836-.042.02a.75.75 0 11-.671-1.34l.041-.022zM12 9a.75.75 0 100-1.5.75.75 0 000 1.5z"
                                            clipRule="evenodd"
                                        />
                                    </svg>
                                    To help students learn more about you, your
                                    bio should reflect your Charisma, Empathy,
                                    Passion, and Personality
                                </Typography>
                            </div>
                        )}
                    />
                </div>
                <div className="flex-1 flex flex-col gap-5">
                    <Controller
                        name="websiteUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Website"
                                    placeholder="https://example.com"
                                    crossOrigin={undefined}
                                    error={!!errors[field.name]}
                                />
                                {errors[field.name] && (
                                    <Typography
                                        variant="small"
                                        color="red"
                                        className="mt-2 flex items-center gap-1 font-normal"
                                    >
                                        {errors[
                                            field.name
                                        ]?.message?.toString()}
                                    </Typography>
                                )}
                            </div>
                        )}
                    />
                    <Controller
                        name="youtubeUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Youtube"
                                    placeholder="http://www.youtube.com/yourusername"
                                    crossOrigin={undefined}
                                    error={!!errors[field.name]}
                                />
                                {errors[field.name] && (
                                    <Typography
                                        variant="small"
                                        color="red"
                                        className="mt-2 flex items-center gap-1 font-normal"
                                    >
                                        {errors[
                                            field.name
                                        ]?.message?.toString()}
                                    </Typography>
                                )}
                            </div>
                        )}
                    />
                    <Controller
                        name="facebookUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Facebook"
                                    placeholder="http://www.facebook.com/yourusername"
                                    crossOrigin={undefined}
                                    error={!!errors[field.name]}
                                />
                                {errors[field.name] && (
                                    <Typography
                                        variant="small"
                                        color="red"
                                        className="mt-2 flex items-center gap-1 font-normal"
                                    >
                                        {errors[
                                            field.name
                                        ]?.message?.toString()}
                                    </Typography>
                                )}
                            </div>
                        )}
                    />
                    <Controller
                        name="linkedInUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="LinkedIn"
                                    placeholder="http://www.linkedin.com/id"
                                    crossOrigin={undefined}
                                    error={!!errors[field.name]}
                                />
                                {errors[field.name] && (
                                    <Typography
                                        variant="small"
                                        color="red"
                                        className="mt-2 flex items-center gap-1 font-normal"
                                    >
                                        {errors[
                                            field.name
                                        ]?.message?.toString()}
                                    </Typography>
                                )}
                            </div>
                        )}
                    />
                </div>
            </div>
            <div>
                <Button
                    type="submit"
                    loading={isUpdating}
                    onClick={handleSubmit(onSubmit)}
                    className="bg-primary flex justify-center mt-5"
                >
                    Save
                </Button>
            </div>
        </form>
    );
}
