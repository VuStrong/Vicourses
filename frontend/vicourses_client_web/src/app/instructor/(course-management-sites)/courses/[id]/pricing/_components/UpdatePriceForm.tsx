"use client";

import { Select, Option, Button } from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useState } from "react";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import { Course } from "@/libs/types/course";
import { updateCourse } from "@/services/api/course";

const prices = [
    "0",
    "19.99",
    "22.99",
    "24.99",
    "27.99",
    "29.99",
    "39.99",
    "49.99",
    "59.99",
];

export default function UpdatePriceForm({ course }: { course: Course }) {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const { data: session } = useSession();

    const {
        handleSubmit,
        control,
        reset,
        formState: { isDirty },
    } = useForm<FieldValues>({
        defaultValues: {
            price: `${course.price}`,
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        if (isUpdating) return;

        setIsUpdating(true);

        try {
            await updateCourse(
                course.id,
                { price: data.price },
                session?.accessToken || ""
            );

            toast.success("Course price saved");
            reset(data);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    return (
        <form className="my-5">
            <Controller
                name="price"
                control={control}
                render={({ field }) => (
                    <Select
                        value={field.value}
                        onChange={field.onChange}
                        label="Select price"
                    >
                        {prices.map((price) => (
                            <Option key={price} value={price}>
                                {price === "0" ? "Free" : `$${price}`}
                            </Option>
                        ))}
                    </Select>
                )}
            />

            <Button
                className="bg-primary mt-5"
                loading={isUpdating}
                disabled={!isDirty || isUpdating}
                type="submit"
                onClick={handleSubmit(onSubmit)}
            >
                Save
            </Button>
        </form>
    );
}
