"use client";

import {
    Button,
    Dialog,
    DialogBody,
    DialogFooter,
    DialogHeader,
    Input,
    Typography,
} from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useState } from "react";
import {
    Controller,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import { Course } from "@/libs/types/course";
import { createCoupon } from "@/services/api/coupon";

type FormValues = {
    days: string;
    availability: string;
    discount: string;
}

export default function CreateCouponModal({
    course,
    open,
    onClose,
}: {
    course: Course;
    open: boolean;
    onClose: () => void;
}) {
    const [isCreating, setIsCreating] = useState<boolean>(false);
    const { data: session } = useSession();
    const router = useRouter();

    const { handleSubmit, control, reset } = useForm<FormValues>({
        defaultValues: {
            days: "0",
            availability: "0",
            discount: "0",
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        if (isCreating) return;

        setIsCreating(true);

        try {
            await createCoupon(
                {
                    courseId: course.id,
                    days: +data.days,
                    availability: +data.availability,
                    discount: +data.discount,
                },
                session?.accessToken || ""
            );

            toast.success("Coupon created");
            router.refresh();
            handleClose();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsCreating(false);
    };

    const handleClose = () => {
        if (!isCreating) {
            reset();
        }
        onClose();
    };

    return (
        <Dialog open={open} handler={handleClose}>
            <DialogHeader>Create a coupon</DialogHeader>
            <DialogBody>
                <div className="flex flex-col md:flex-row flex-wrap gap-5">
                    <Controller
                        name="days"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Required",
                            },
                            min: {
                                value: 1,
                                message: "Must be between 1 and 15",
                            },
                            max: {
                                value: 15,
                                message: "Must be between 1 and 15",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div className="flex-grow">
                                <label
                                    htmlFor="title"
                                    className="text-black font-bold flex gap-1"
                                >
                                    Days
                                </label>
                                <Input
                                    {...field}
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    type="number"
                                />
                                <div></div>
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
                        name="availability"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Required",
                            },
                            min: {
                                value: 1,
                                message: "Must be between 1 and 1000",
                            },
                            max: {
                                value: 1000,
                                message: "Must be between 1 and 1000",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div className="flex-grow">
                                <label
                                    htmlFor="title"
                                    className="text-black font-bold flex gap-1"
                                >
                                    Availability
                                </label>
                                <Input
                                    {...field}
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    type="number"
                                />
                                <div></div>
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
                        name="discount"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Required",
                            },
                            min: {
                                value: 5,
                                message: "Must be between 5 and 90",
                            },
                            max: {
                                value: 90,
                                message: "Must be between 5 and 90",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div className="flex-grow">
                                <label
                                    htmlFor="title"
                                    className="text-black font-bold flex gap-1"
                                >
                                    Discount
                                </label>
                                <Input
                                    {...field}
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    type="number"
                                />
                                <div></div>
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
            </DialogBody>
            <DialogFooter>
                <Button variant="text" onClick={handleClose} className="mr-1">
                    <span>Cancel</span>
                </Button>
                <Button
                    className="bg-primary"
                    loading={isCreating}
                    onClick={handleSubmit(onSubmit)}
                >
                    <span>Create</span>
                </Button>
            </DialogFooter>
        </Dialog>
    );
}
