"use client";

import { Category } from "@/libs/types/category";
import { getCategories } from "@/services/api/category";
import { createCourse } from "@/services/api/course";
import {
    Button,
    Dialog,
    DialogBody,
    DialogFooter,
    DialogHeader,
    Input,
    Select,
    Typography,
    Option,
} from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";

export default function CreateCourseModal({
    open,
    onClose,
}: {
    open: boolean;
    onClose: () => void;
}) {
    const [isCreating, setIsCreating] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>();
    const [subCategories, setSubCategories] = useState<Category[]>();
    const { data: session } = useSession();
    const router = useRouter();

    const { handleSubmit, reset, control, watch, setValue } =
        useForm<FieldValues>({
            defaultValues: {
                title: "",
                categoryId: "",
                subCategoryId: "",
            },
        });
    const categoryId = watch("categoryId");

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        if (isCreating) return;

        setIsCreating(true);

        try {
            const createdCourse = await createCourse(
                {
                    title: data.title,
                    categoryId: data.categoryId,
                    subCategoryId: data.subCategoryId,
                },
                session?.accessToken || ""
            );

            toast.success(`Course ${createdCourse.title} created`);
            router.push(`/instructor/courses/${createdCourse.id}`);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsCreating(false);
    };

    const handleClose = () => {
        if (!isCreating) {
            setSubCategories(undefined);
            reset();
        }
        onClose();
    };

    useEffect(() => {
        (async () => {
            const result = await getCategories({
                parentId: "null",
            });

            setCategories(result);
        })();
    }, []);

    // Fetch sub categories when categoryId change
    useEffect(() => {
        if (categoryId) {
            (async () => {
                setValue("subCategoryId", "");
                setSubCategories(undefined);

                const result = await getCategories({
                    parentId: categoryId,
                });

                setSubCategories(result);
            })();
        }
    }, [categoryId]);

    return (
        <Dialog open={open} size={"lg"} handler={handleClose}>
            <DialogHeader>Create course</DialogHeader>
            <DialogBody>
                <form
                    onSubmit={handleSubmit(onSubmit)}
                    className="flex flex-col gap-5"
                >
                    <Controller
                        name="title"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Enter course title.",
                            },
                            minLength: {
                                value: 5,
                                message:
                                    "Title must be between 5 and 60 characters",
                            },
                            maxLength: {
                                value: 60,
                                message:
                                    "Title must be between 5 and 60 characters",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <Input
                                    {...field}
                                    label="Title"
                                    placeholder="Example: From ReactJS to jobless - ULTIMATE course"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
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
                        name="categoryId"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Select course's category.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <Select
                                    {...field}
                                    disabled={!categories}
                                    error={!!fieldState.error}
                                    label="Select category"
                                >
                                    {categories ? (
                                        categories.map((category) => (
                                            <Option
                                                key={category.id}
                                                value={category.id}
                                            >
                                                {category.name}
                                            </Option>
                                        ))
                                    ) : (
                                        <Option>NONE</Option>
                                    )}
                                </Select>
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
                        name="subCategoryId"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Select course's subcategory.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div>
                                <Select
                                    {...field}
                                    error={!!fieldState.error}
                                    selected={() =>
                                        subCategories?.find(
                                            (c) => c.id === field.value
                                        )?.name
                                    }
                                    disabled={!subCategories}
                                    label="Select subcategory"
                                >
                                    {subCategories ? (
                                        subCategories.map((subcategory) => (
                                            <Option
                                                key={subcategory.id}
                                                value={subcategory.id}
                                            >
                                                {subcategory.name}
                                            </Option>
                                        ))
                                    ) : (
                                        <Option>NONE</Option>
                                    )}
                                </Select>
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
                </form>
            </DialogBody>
            <DialogFooter>
                <Button variant="text" onClick={handleClose} className="mr-1">
                    <span>Cancel</span>
                </Button>
                <Button
                    loading={isCreating}
                    onClick={handleSubmit(onSubmit)}
                    className="bg-primary"
                >
                    <span>Confirm</span>
                </Button>
            </DialogFooter>
        </Dialog>
    );
}
