"use client";

import { useState } from "react";
import toast from "react-hot-toast";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import {
    Button,
    Card,
    CardBody,
    CardFooter,
    Dialog,
    Input,
    Typography,
} from "@material-tailwind/react";

import { changePassword } from "@/services/api/user";

export default function ChangePasswordModal({
    isOpen,
    onClose,
    accessToken,
}: {
    isOpen: boolean;
    onClose: () => void;
    accessToken: string;
}) {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);

    const {
        handleSubmit,
        getValues,
        reset,
        control,
        formState: { errors },
    } = useForm<FieldValues>({
        defaultValues: {
            oldPassword: "",
            newPassword: "",
            newPasswordConfirm: "",
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsUpdating(true);

        try {
            await changePassword(
                data.oldPassword,
                data.newPassword,
                accessToken
            );

            toast.success("Password changed");
            handleClose();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    const handleClose = () => {
        reset();
        onClose();
    };

    return (
        <Dialog
            size="xs"
            open={isOpen}
            handler={handleClose}
            className="bg-transparent shadow-none"
        >
            <Card className="mx-auto w-full max-w-[24rem]">
                <CardBody className="flex flex-col gap-4">
                    <Typography variant="h4" color="blue-gray">
                        Change password
                    </Typography>

                    <Typography className="-mb-2" variant="h6">
                        Current password
                    </Typography>
                    <Controller
                        name="oldPassword"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter your current password.",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Current password"
                                    type="password"
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

                    <Typography className="-mb-2" variant="h6">
                        New Password
                    </Typography>
                    <Controller
                        name="newPassword"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter your new password.",
                            },
                            minLength: {
                                value: 8,
                                message:
                                    "New password must be between 8 and 50 characters.",
                            },
                            maxLength: {
                                value: 50,
                                message:
                                    "New password must be between 8 and 50 characters.",
                            },
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="New password"
                                    type="password"
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

                    <Typography className="-mb-2" variant="h6">
                        Confirm new password
                    </Typography>
                    <Controller
                        name="newPasswordConfirm"
                        control={control}
                        rules={{
                            validate: (value) =>
                                value === getValues("newPassword") ||
                                "New password not match",
                        }}
                        render={({ field }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Current password"
                                    type="password"
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
                </CardBody>
                <CardFooter className="pt-0">
                    <Button
                        className="bg-primary flex justify-center mt-5"
                        onClick={handleSubmit(onSubmit)}
                        loading={isUpdating}
                        fullWidth
                    >
                        Confirm
                    </Button>
                </CardFooter>
            </Card>
        </Dialog>
    );
}
