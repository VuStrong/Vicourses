"use client";

import { Category } from "@/libs/types/category";
import { CourseDetail } from "@/libs/types/course";
import { getCategories } from "@/services/api/category";
import {
    Button,
    Input,
    Select,
    Typography,
    Option,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import {
    Controller,
    FieldValues,
    SubmitHandler,
    useForm,
} from "react-hook-form";

export default function UpdateCourseOverviewForm({
    course,
}: {
    course: CourseDetail;
}) {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>();
    const [subCategories, setSubCategories] = useState<Category[]>();

    const { handleSubmit, control, setValue } = useForm<FieldValues>({
        defaultValues: {
            title: course.title,
            description: course.description,
            level: course.level.toString(),
            categoryId: course.category.id,
            subCategoryId: course.subCategory.id,
            tags: course.tags,
        },
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        setIsUpdating(true);

        console.log(data);

        setIsUpdating(false);
    };

    useEffect(() => {
        (async () => {
            const result = await getCategories({
                parentId: "null",
            });

            setCategories(result);
        })();

        (async () => {
            const result = await getCategories({
                parentId: course.category.id,
            });

            setSubCategories(result);
        })();
    }, []);

    const handleCategoryIdChange = async (categoryId: string) => {
        setValue("categoryId", categoryId);
        setValue("subCategoryId", "");

        const result = await getCategories({
            parentId: categoryId,
        });

        setSubCategories(result);
    };

    return (
        <form className="flex flex-col gap-5 my-10">
            <div className="w-full md:max-w-[200px] mb-5">
                <Button
                    className="bg-primary"
                    fullWidth
                    type="submit"
                    onClick={handleSubmit(onSubmit)}
                >
                    Save
                </Button>
            </div>

            <Controller
                name="title"
                control={control}
                rules={{
                    required: {
                        value: true,
                        message: "Title must be between 5 and 60 characters",
                    },
                    minLength: {
                        value: 5,
                        message: "Title must be between 5 and 60 characters",
                    },
                    maxLength: {
                        value: 60,
                        message: "Title must be between 5 and 60 characters",
                    },
                }}
                render={({ field, fieldState }) => (
                    <div>
                        <label htmlFor="title" className="text-black font-bold">
                            Title
                        </label>
                        <Input
                            id="title"
                            {...field}
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

            <div>
                <div className="text-black font-bold">Basic information</div>
                <div className="flex flex-col md:flex-row gap-5 flex-wrap">
                    <div className="flex-1">
                        <Controller
                            name="level"
                            control={control}
                            render={({ field }) => (
                                <Select {...field} label="Level">
                                    <Option value="All">All</Option>
                                    <Option value="Basic">Basic</Option>
                                    <Option value="Intermediate">
                                        Intermediate
                                    </Option>
                                    <Option value="Expert">Expert</Option>
                                </Select>
                            )}
                        />
                    </div>
                    <div className="flex-1">
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
                                        value={field.value}
                                        onChange={(value) =>
                                            handleCategoryIdChange(value || "")
                                        }
                                        selected={() =>
                                            categories?.find(
                                                (c) => c.id === field.value
                                            )?.name
                                        }
                                        error={!!fieldState.error}
                                        disabled={!categories}
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
                    </div>
                    <div className="flex-1">
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
                                        selected={() =>
                                            subCategories?.find(
                                                (c) => c.id === field.value
                                            )?.name
                                        }
                                        disabled={!subCategories}
                                        error={!!fieldState.error}
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
                    </div>
                </div>
            </div>
        </form>
    );
}
