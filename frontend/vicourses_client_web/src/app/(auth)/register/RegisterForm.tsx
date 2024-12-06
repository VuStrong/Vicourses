"use client";

import { signIn } from "next-auth/react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useState } from "react";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import { Button } from "@material-tailwind/react";
import { ReactHookFormInput } from "../../../components/common";
import { register as registerUser } from "@/services/api/auth";

export default function RegisterForm() {
    const router = useRouter();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const {
        register,
        handleSubmit,
        getValues,
        formState: { errors },
    } = useForm<FieldValues>({
        defaultValues: {
            name: "",
            email: "",
            password: "",
            passwordConfirm: "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsLoading(true);

        try {
            await registerUser(data.name, data.email, data.password);

            const signInResponse = await signIn("credentials", {
                email: data.email,
                password: data.password,
                redirect: false,
            });

            if (!signInResponse?.code) {
                router.push("/choose-categories");
                router.refresh();
            } else {
                toast.error("Invalid credentials!");
            }
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsLoading(false);
    };

    return (
        <form className="lg:h-auto md:h-auto border-0 rounded-lg shadow-lg flex flex-col w-full bg-white outline-none focus:outline-none">
            {/* BODY */}
            <div className="relative p-6 flex-auto">
                <div className="flex flex-col gap-4">
                    <div className="text-start">
                        <div className="text-xl md:text-2xl font-bold text-primary mb-2">
                            Register and start learning
                        </div>
                    </div>
                    <ReactHookFormInput
                        id="name"
                        label="Name"
                        disabled={isLoading}
                        register={register("name", {
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
                        })}
                        errors={errors}
                    />
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
                            minLength: {
                                value: 8,
                                message:
                                    "Password must be between 8 and 50 characters",
                            },
                            maxLength: {
                                value: 50,
                                message:
                                    "Password must be between 8 and 50 characters",
                            },
                        })}
                        errors={errors}
                    />
                    <ReactHookFormInput
                        id="passwordConfirm"
                        label="Confirm password"
                        type="password"
                        disabled={isLoading}
                        register={register("passwordConfirm", {
                            validate: (value) =>
                                value === getValues("password") ||
                                "Password not match",
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
                        Sign up
                    </Button>
                    <small className="text-gray-700">
                        By signing up, you agree to the 
                        <Link href={"#"} className="px-1 underline">Terms of Use</Link> 
                        and 
                        <Link href={"#"} className="px-1 underline">Privacy Policy.</Link> 
                    </small>
                </div>
                <div className="flex flex-col gap-4 mt-3">
                    <hr className="border-black" />
                    <div className="text-gray-900 text-center mt-4 font-light">
                        <div className="flex flex-row justify-center items-center gap-2">
                            <div>Already have an account?</div>
                            <Link href="/login" className="underline">
                                Sign In
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
