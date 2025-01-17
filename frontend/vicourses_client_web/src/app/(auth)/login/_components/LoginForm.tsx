"use client";

import { signIn } from "next-auth/react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";
import { useState } from "react";
import { FcGoogle } from "react-icons/fc";
import {
    Controller,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import { Button, Input, Typography } from "@material-tailwind/react";

type FormValues = {
    email: string;
    password: string;
}

export default function LoginForm() {
    const searchParams = useSearchParams();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const { handleSubmit, control } = useForm<FormValues>({
        defaultValues: {
            email: "",
            password: "",
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        setIsLoading(true);

        const res = await signIn("credentials", { ...data, redirect: false });

        if (!res?.code) {
            const pageToRedirect = searchParams?.get("callbackUrl") ?? "/";

            window.location.href = decodeURIComponent(pageToRedirect);
        } else {
            toast.error("Invalid credentials!");
            setIsLoading(false);
        }
    };

    const handleGoogleLogin = () => {
        const pageToRedirect = searchParams?.get("callbackUrl") ?? "/";

        signIn("google", { redirectTo: decodeURIComponent(pageToRedirect) });
    };

    return (
        <form className="lg:h-auto md:h-auto border-0 rounded-lg shadow-2xl flex flex-col w-full bg-white outline-none focus:outline-none">
            {/* BODY */}
            <div className="relative p-6 flex-auto">
                <div className="flex flex-col gap-4">
                    <div className="text-start">
                        <div className="text-xl md:text-2xl font-bold text-primary mb-2">
                            Sign in to Vicourses
                        </div>
                    </div>
                    <Controller
                        name="email"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter email.",
                            },
                            pattern: {
                                value: /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/,
                                message: "Email is invalid.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Email"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    size="lg"
                                    type="email"
                                    disabled={isLoading}
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
                        name="password"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter password.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Password"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    size="lg"
                                    type="password"
                                    disabled={isLoading}
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

            {/* FOOTER */}
            <div className="flex flex-col gap-2 p-6">
                <div className="flex flex-col items-center gap-4 w-full">
                    <Button
                        type="submit"
                        fullWidth
                        loading={isLoading}
                        onClick={handleSubmit(onSubmit)}
                        className="bg-primary flex justify-center"
                    >
                        Sign in
                    </Button>

                    <Link
                        href="/forgot-password"
                        className="underline text-gray-900"
                    >
                        Forgot password?
                    </Link>
                </div>
                <div className="flex flex-col gap-4 mt-3">
                    <hr className="border-black" />
                    <Button
                        fullWidth
                        variant="outlined"
                        loading={isLoading}
                        onClick={handleGoogleLogin}
                        className="flex items-center justify-center gap-3 bg-transparent text-gray-900"
                    >
                        <FcGoogle size={24} />
                        Sign in with Google
                    </Button>
                    <div className="text-gray-900 text-center mt-4 font-light">
                        <div className="flex flex-row justify-center items-center gap-2">
                            <div>Don't have account?</div>
                            <Link href="/register" className="underline">
                                Sign Up
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
