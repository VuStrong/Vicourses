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
import { v4 as uuidv4 } from "uuid";
import dynamic from "next/dynamic";
import "react-quill/dist/quill.snow.css";
import CourseInfoTooltip from "./CourseInfoTooltip";
import toast from "react-hot-toast";
import { updateCourse } from "@/services/api/course";
import { useSession } from "next-auth/react";
import { Locale } from "@/libs/types/common";
import { getLocales } from "@/services/api/locale";
import CourseThumbnailUpload from "./CourseThumbnailUpload";
import { getFileExtension } from "@/libs/utils";
import { uploadImage } from "@/services/api/storage";

const ReactQuill = dynamic(() => import("react-quill"), { ssr: false });

const getWordCount = (text: string) => {
    const plainText = text?.replace(/<[^>]+>/g, "").trim();
    if (!plainText) return 0;
    return plainText.split(/\s+/).length;
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
    const { data: session } = useSession();

    const {
        handleSubmit,
        control,
        setValue,
        reset,
        formState: { isDirty },
    } = useForm<FieldValues>({
        defaultValues: {
            title: course.title,
            description: course.description,
            level: course.level.toString(),
            categoryId: course.category.id,
            subCategoryId: course.subCategory.id,
            tags: course.tags,
            locale: course.locale?.name || null,
            thumbnail: null,
        },
        mode: "onSubmit",
    });

    const onSubmit: SubmitHandler<FieldValues> = async (data) => {
        if (isUpdating) return;

        setIsUpdating(true);
        const descriptionWordCount = getWordCount(data.description);
        if (descriptionWordCount === 0) {
            data.description = null;
        }

        try {
            // Upload image first
            let thumbnailToken = undefined;
            if (data.thumbnail) {
                const fileId = `vicourses-course-photos/${uuidv4()}.${getFileExtension(
                    data.thumbnail
                )}`;

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

        // Fetch locales for select tag
        (async () => {
            const result = await getLocales();

            setLocales(result);
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
                    rules={{
                        validate: (value: string) => {
                            const wordCount = getWordCount(value);

                            if (wordCount === 0 || wordCount >= 100)
                                return true;

                            const remain = 100 - wordCount;
                            return `Description must have at least 100 words, write ${remain} more words`;
                        },
                    }}
                    render={({ field, fieldState }) => (
                        <>
                            <ReactQuill
                                className={`${
                                    !!fieldState.error && "border border-error"
                                }`}
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
                            {fieldState.error && (
                                <Typography
                                    variant="small"
                                    color="red"
                                    className="mt-2 font-normal"
                                >
                                    {fieldState.error.message as string}
                                </Typography>
                            )}
                        </>
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
        </form>
    );
}
