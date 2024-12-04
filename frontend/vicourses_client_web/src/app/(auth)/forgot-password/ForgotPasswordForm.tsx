"use client";

import { ReactHookFormInput } from "@/components/common";
import { sendPasswordResetLink } from "@/services/api/auth";
import { Button } from "@material-tailwind/react";
import { useState } from "react";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { toast } from "react-hot-toast";

export default function ForgotPasswordForm() {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<FieldValues>({
        defaultValues: {
            email: "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsLoading(true);

        try {
            await sendPasswordResetLink(data.email);
            toast.success("Email has been sent");
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
                    <div className="text-center">
                        <div className="text-xl md:text-2xl font-bold text-primary mb-2">
                            Enter your email
                        </div>
                        <p className="text-gray-900">
                            Enter the email address you used to register your account. 
                            We'll send an email with a link to reset your password.
                        </p>
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
                </div>
            </div>

            <div className="flex flex-col p-6">
                <div className="flex flex-col items-center w-full">
                    <Button
                        type="submit"
                        fullWidth
                        loading={isLoading}
                        onClick={handleSubmit(onSubmit)}
                        className="bg-primary flex justify-center"
                    >
                        Send
                    </Button>
                </div>
            </div>
        </form>
    );
}