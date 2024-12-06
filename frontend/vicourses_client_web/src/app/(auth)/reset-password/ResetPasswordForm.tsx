"use client"

import { ReactHookFormInput } from "@/components/common";
import { resetPassword } from "@/services/api/auth";
import { Button } from "@material-tailwind/react";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import toast from "react-hot-toast";

export default function ResetPasswordForm({
    userId,
    token,
}: {
    userId: string;
    token: string;
}) {
    const router = useRouter();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const {
        register,
        handleSubmit,
        getValues,
        formState: { errors },
    } = useForm<FieldValues>({
        defaultValues: {
            password: "",
            passwordConfirm: "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsLoading(true);

        try {
            await resetPassword(userId, token, data.password);

            toast.success("Your password has been reset");
            router.push("/login");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsLoading(false);
    };

    return (
        <form className="lg:h-auto md:h-auto border-0 rounded-lg shadow-2xl flex flex-col w-full bg-white outline-none focus:outline-none">
            {/* BODY */}
            <div className="relative p-6 flex-auto">
                <div className="flex flex-col gap-4">
                    <div className="text-start">
                        <div className="text-xl md:text-2xl font-bold text-primary mb-2">
                            Reset password
                        </div>
                    </div>
                    <ReactHookFormInput
                        id="password"
                        label="New password"
                        type="password"
                        disabled={isLoading}
                        register={register("password", {
                            required: {
                                value: true,
                                message: "Enter new password.",
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

            <div className="flex flex-col gap-2 p-6">
                <div className="flex flex-col items-center gap-4 w-full">
                    <Button
                        type="submit"
                        fullWidth
                        loading={isLoading}
                        onClick={handleSubmit(onSubmit)}
                        className="bg-primary flex justify-center"
                    >
                        Confirm
                    </Button>
                </div>
            </div>
        </form>
    );
}