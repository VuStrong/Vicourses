"use client";

import path from "path";
import { v4 as uuidv4 } from "uuid";
import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import dynamic from "next/dynamic";
import {
    Button,
    Input,
    Select,
    Typography,
    Option,
} from "@material-tailwind/react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import "react-quill/dist/quill.snow.css";
import toast from "react-hot-toast";
import CreatableSelect from "react-select/creatable";

import { Locale } from "@/libs/types/common";
import { Category } from "@/libs/types/category";
import { CourseDetail, CourseLevel } from "@/libs/types/course";
import { updateCourse } from "@/services/api/course";
import { getLocales } from "@/services/api/locale";
import { uploadImage } from "@/services/api/storage";
import { getCategories } from "@/services/api/category";
import CourseInfoTooltip from "./CourseInfoTooltip";
import CourseThumbnailUpload from "./CourseThumbnailUpload";
import CoursePreviewVideoUpload from "./CoursePreviewVideoUpload";

const ReactQuill = dynamic(() => import("react-quill"), { ssr: false });

const loadTagOptions = async (inputValue: string) => {
    const res = await fetch(`/api/tags?q=${inputValue}`);

    if (!res.ok) return [];

    const data = await res.json();

    return data.tags.map((tag: string) => ({
        value: tag,
        label: tag,
    }));
};

type FormValues = {
    title: string;
    description?: string;
    level: CourseLevel;
    categoryId: string;
    subCategoryId: string;
    tags: {
        value: string;
        label: string;
    }[];
    locale?: string;
    thumbnail: File | null;
};

export default function UpdateCourseOverviewForm({
    course,
}: {
    course: CourseDetail;
}) {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>();
    const [subCategories, setSubCategories] = useState<Category[]>();
    const [locales, setLocales] = useState<Locale[]>();
    const [courseTagOptions, setCourseTagOptions] = useState<
        {
            value: string;
            label: string;
        }[]
    >([]);
    const { data: session } = useSession();

    const {
        handleSubmit,
        control,
        setValue,
        reset,
        formState: { isDirty },
    } = useForm<FormValues>({
        defaultValues: {
            title: course.title,
            description: course.description || undefined,
            level: course.level,
            categoryId: course.category.id,
            subCategoryId: course.subCategory.id,
            tags: course.tags.map((tag) => ({
                value: tag,
                label: tag,
            })),
            locale: course.locale?.name,
            thumbnail: null,
        },
        mode: "onSubmit",
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        if (isUpdating) return;

        setIsUpdating(true);

        try {
            // Upload image first
            let thumbnailToken = undefined;
            if (data.thumbnail) {
                const ext = path.extname(data.thumbnail.name);
                const fileId = `images/vicourses-course-photos/${uuidv4()}${ext}`;

                const uploadResponse = await uploadImage(
                    data.thumbnail,
                    session?.accessToken || "",
                    fileId
                );
                thumbnailToken = uploadResponse.token;
                data.thumbnail = null;
            }

            await updateCourse(
                course.id,
                {
                    ...data,
                    thumbnailToken,
                    tags: data.tags.map((option) => option.value),
                },
                session?.accessToken || ""
            );

            toast.success("Course saved");
            reset(data);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    useEffect(() => {
        // Fetch root categories
        (async () => {
            const result = await getCategories({
                parentId: "null",
            });

            setCategories(result);
        })();

        // Fetch sub categories base on current course category
        (async () => {
            const result = await getCategories({
                parentId: course.category.id,
            });

            setSubCategories(result);
        })();

        // Fetch locales
        (async () => {
            const result = await getLocales();

            setLocales(result);
        })();

        // Fetch course tags
        (async () => {
            const result = await fetch("/data/tags.json");

            if (result.ok) {
                const tags = (await result.json()) as string[];
                setCourseTagOptions(
                    tags.map((tag) => ({
                        value: tag,
                        label: tag,
                    }))
                );
            }
        })();
    }, []);

    const handleCategoryIdChange = async (categoryId: string) => {
        setValue("categoryId", categoryId, { shouldDirty: true });
        setValue("subCategoryId", "");

        const result = await getCategories({
            parentId: categoryId,
        });

        setSubCategories(result);
    };

    return (
        <form className="flex flex-col gap-5 mb-10 mt-5">
            <div className="w-full md:max-w-[200px] mb-2">
                <Button
                    className="bg-primary rounded-none flex justify-center"
                    fullWidth
                    disabled={!isDirty || isUpdating}
                    loading={isUpdating}
                    type="submit"
                    onClick={handleSubmit(onSubmit)}
                >
                    Save
                </Button>
            </div>

            {/* Title input */}
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
                        <label
                            htmlFor="title"
                            className="text-black font-bold flex gap-1"
                        >
                            Title
                            <span>
                                <CourseInfoTooltip
                                    content="Your title must not only be attention-grabbing and
                            informative, but also optimized for searchability"
                                />
                            </span>
                        </label>
                        <Input
                            id="title"
                            {...field}
                            crossOrigin={undefined}
                            error={!!fieldState.error}
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

            {/* Description input */}
            <div>
                <div className="text-black font-bold">Description</div>

                <Controller
                    name="description"
                    control={control}
                    render={({ field }) => (
                        <ReactQuill
                            theme="snow"
                            placeholder="Write your course description"
                            value={field.value}
                            onChange={field.onChange}
                            modules={{
                                toolbar: [
                                    "bold",
                                    "italic",
                                    { list: "ordered" },
                                    { list: "bullet" },
                                ],
                            }}
                        />
                    )}
                />
            </div>

            <div>
                <div className="text-black font-bold">Basic information</div>
                <div className="flex flex-col md:flex-row gap-5 flex-wrap">
                    <div className="flex-1">
                        {/* CourseLevel select */}
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
                        {/* Course Locale select */}
                        <Controller
                            name="locale"
                            control={control}
                            render={({ field }) => (
                                <Select
                                    value={field.value}
                                    onChange={field.onChange}
                                    selected={() =>
                                        locales?.find(
                                            (l) => l.name === field.value
                                        )?.englishTitle
                                    }
                                    disabled={!locales}
                                    label="Select locale"
                                >
                                    {locales ? (
                                        locales.map((locale) => (
                                            <Option
                                                key={locale.name}
                                                value={locale.name}
                                            >
                                                {locale.englishTitle}
                                            </Option>
                                        ))
                                    ) : (
                                        <Option>NONE</Option>
                                    )}
                                </Select>
                            )}
                        />
                    </div>
                    <div className="flex-1">
                        {/* Category select */}
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
                        {/* SubCategory select */}
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

            {/* Course tags select */}
            <div>
                <div className="text-black font-bold flex gap-1">
                    Tags
                    <span>
                        <CourseInfoTooltip content='Each individual tag you choose should comprehensively describe the course content but not be too spread out. For example: "Comprehensive reactjs course" must contain "reactjs"' />
                    </span>
                </div>

                <Controller
                    name="tags"
                    control={control}
                    render={({ field }) => (
                        <CreatableSelect
                            instanceId="tags"
                            placeholder="Example: nodejs"
                            isMulti
                            value={field.value}
                            onChange={field.onChange}
                            options={courseTagOptions}
                        />
                    )}
                />
            </div>

            <div>
                <div className="text-black font-bold">Course thumbnail</div>

                <CourseThumbnailUpload
                    course={course}
                    onImageChange={(file) => {
                        setValue("thumbnail", file || null, {
                            shouldDirty: true,
                        });
                    }}
                />
            </div>

            <div>
                <div className="text-black font-bold">Preview video</div>

                <CoursePreviewVideoUpload initialCourse={course} />
            </div>
        </form>
    );
}
