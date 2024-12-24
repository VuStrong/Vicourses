"use client";

import { useEffect, useState } from "react";
import dynamic from "next/dynamic";
import { useSession } from "next-auth/react";
import {
    Button,
    Input,
    Tooltip,
    Typography,
} from "@material-tailwind/react";
import {
    Controller,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import "react-quill/dist/quill.snow.css";

import { User } from "@/libs/types/user";
import { Loader } from "@/components/common";
import { getAuthenticatedUser, updateProfile } from "@/services/api/user";

const ReactQuill = dynamic(() => import("react-quill"), { ssr: false });

const urlRegex =
    /(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g;

type FormValues = {
    name: string;
    headline?: string;
    description?: string;
    websiteUrl?: string;
    youtubeUrl?: string;
    facebookUrl?: string;
    linkedInUrl?: string;
}

export default function UpdateProfileForm() {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [user, setUser] = useState<User | null>(null);
    const { data: session, status } = useSession();

    const {
        handleSubmit,
        control,
        reset,
        formState: { isDirty },
    } = useForm<FormValues>();

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        if (isUpdating) return;

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
            reset(data);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    useEffect(() => {
        (async () => {
            if (status === "authenticated") {
                const result = await getAuthenticatedUser(session.accessToken);

                setUser(result);

                if (result) {
                    const formData = {
                        name: result.name,
                        headline: result.headline || "",
                        description: result.description || "",
                        websiteUrl: result.websiteUrl || "",
                        youtubeUrl: result.youtubeUrl || "",
                        facebookUrl: result.facebookUrl || "",
                        linkedInUrl: result.linkedInUrl || "",
                    };

                    reset(formData);
                }
            }
        })();
    }, [status]);

    if (!user) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <form className="my-5 w-full">
            <div className="flex flex-col md:flex-row flex-wrap gap-3">
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
                        render={({ field, fieldState }) => (
                            <div>
                                <label
                                    htmlFor="name"
                                    className="text-gray-800 font-semibold flex gap-1"
                                >
                                    Name
                                </label>
                                <Input
                                    id="name"
                                    {...field}
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    className="placeholder:opacity-100"
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
                        render={({ field, fieldState }) => (
                            <div>
                                <label
                                    htmlFor="headline"
                                    className="text-gray-800 font-semibold flex gap-1"
                                >
                                    Headline
                                </label>
                                <Input
                                    {...field}
                                    id="headline"
                                    placeholder="Instructor at Vicourses"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    className="placeholder:opacity-100"
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

                    <div>
                        <div className="text-gray-800 font-semibold flex gap-1">
                            Description
                            <span>
                                <ProfileInfoTooltip
                                    content="To help students learn more about you,
                                        your bio should reflect your Charisma,
                                        Empathy, Passion, and Personality"
                                />
                            </span>
                        </div>
                        <Controller
                            name="description"
                            control={control}
                            render={({ field }) => (
                                <ReactQuill
                                    theme="snow"
                                    placeholder="Write your profile description"
                                    value={field.value}
                                    onChange={field.onChange}
                                    modules={{
                                        toolbar: ["bold", "italic"],
                                    }}
                                />
                            )}
                        />
                    </div>
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
                        render={({ field, fieldState }) => (
                            <div>
                                <label
                                    htmlFor="websiteUrl"
                                    className="text-gray-800 font-semibold flex gap-1"
                                >
                                    Website
                                </label>
                                <Input
                                    {...field}
                                    id="websiteUrl"
                                    placeholder="https://example.com"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    className="placeholder:opacity-100"
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
                    <Controller
                        name="youtubeUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <label
                                    htmlFor="youtubeUrl"
                                    className="text-gray-800 font-semibold flex gap-1"
                                >
                                    Youtube
                                </label>
                                <Input
                                    {...field}
                                    id="youtubeUrl"
                                    placeholder="http://www.youtube.com/yourusername"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    className="placeholder:opacity-100"
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
                    <Controller
                        name="facebookUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <label
                                    htmlFor="facebookUrl"
                                    className="text-gray-800 font-semibold flex gap-1"
                                >
                                    Facebook
                                </label>
                                <Input
                                    {...field}
                                    id="facebookUrl"
                                    placeholder="http://www.facebook.com/yourusername"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    className="placeholder:opacity-100"
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
                    <Controller
                        name="linkedInUrl"
                        control={control}
                        rules={{
                            pattern: {
                                value: urlRegex,
                                message: "Url is invalid.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <label
                                    htmlFor="linkedInUrl"
                                    className="text-gray-800 font-semibold flex gap-1"
                                >
                                    LinkedIn
                                </label>
                                <Input
                                    {...field}
                                    id="linkedInUrl"
                                    placeholder="http://www.linkedin.com/id"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    className="placeholder:opacity-100"
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
                </div>
            </div>
            <div>
                <Button
                    type="submit"
                    disabled={!isDirty || isUpdating}
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

function ProfileInfoTooltip({ content }: { content: string }) {
    return (
        <Tooltip
            content={
                <div className="w-80">
                    <Typography
                        variant="small"
                        color="white"
                        className="font-normal opacity-80"
                    >
                        {content}
                    </Typography>
                </div>
            }
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                strokeWidth={2}
                className="h-5 w-5 cursor-pointer text-blue-gray-500"
            >
                <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z"
                />
            </svg>
        </Tooltip>
    );
}
