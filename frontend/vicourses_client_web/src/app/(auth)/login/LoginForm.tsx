"use client";

import { signIn } from "next-auth/react";
import { useRouter, useSearchParams } from "next/navigation";
import Link from "next/link";

import { useState } from "react";
import { FcGoogle } from "react-icons/fc";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import toast from "react-hot-toast";

import { Button } from "@material-tailwind/react";
import { ReactHookFormInput } from "@/components/common";

export default function LoginForm() {
    const router = useRouter();
    const searchParams = useSearchParams();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<FieldValues>({
        defaultValues: {
            email: "",
            password: "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsLoading(true);

        const res = await signIn("credentials", { ...data, redirect: false });
        
        if (!res?.code) {
            router.push(searchParams?.get("callbackUrl") ?? "/");
            router.refresh();
        } else {
            toast.error("Invalid credentials!");
            setIsLoading(false);
        }
    };

    const handleGoogleLogin = () => {
        signIn("google");
    }

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
                    <ReactHookFormInput
                        id="email"
                        label="Email"
                        disabled={isLoading}
                        register={register("email", {
                            required: {
                                value: true,
                                message: "Enter email.",
                            },
                            pattern: {
                                value: /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/,
                                message: "Email is invalid.",
                            },
                        })}
                        errors={errors}
                    />
                    <ReactHookFormInput
                        id="password"
                        label="Password"
                        type="password"
                        disabled={isLoading}
                        register={register("password", {
                            required: {
                                value: true,
                                message: "Enter password.",
                            },
                        })}
                        errors={errors}
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

                    <Link href="/forgot-password" className="underline text-gray-900">
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
