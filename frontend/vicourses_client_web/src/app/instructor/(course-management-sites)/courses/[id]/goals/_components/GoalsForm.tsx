"use client";

import { Button, Input, Typography } from "@material-tailwind/react";
import { useState } from "react";
import {
    Control,
    Controller,
    SubmitHandler,
    useFieldArray,
    useForm,
} from "react-hook-form";
import toast from "react-hot-toast";
import { IoAddOutline } from "react-icons/io5";
import { FaTrash } from "react-icons/fa";
import { useSession } from "next-auth/react";

import { updateCourse } from "@/services/api/course";
import { CourseDetail } from "@/libs/types/course";

const MAX_LEARNED_CONTENTS = 10;
const MAX_REQUIREMENTS = 10;
const MAX_TARGET_STUDENTS = 10;

type FormValues = {
    learnedContents: {
        content: string;
    }[];
    requirements: {
        content: string;
    }[];
    targetStudents: {
        content: string;
    }[];
};

export default function GoalsForm({ course }: { course: CourseDetail }) {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const { data: session } = useSession();

    const {
        control,
        handleSubmit,
        reset,
        formState: { isDirty },
    } = useForm<FormValues>({
        defaultValues: {
            learnedContents: !!course.learnedContents[0]
                ? course.learnedContents.map((c) => ({ content: c }))
                : [{ content: "" }],
            requirements: !!course.requirements[0]
                ? course.requirements.map((c) => ({ content: c }))
                : [{ content: "" }],
            targetStudents: !!course.targetStudents[0]
                ? course.targetStudents.map((c) => ({ content: c }))
                : [{ content: "" }],
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        setIsUpdating(true);

        const learnedContents = data.learnedContents.map((c) => c.content);
        const requirements = data.requirements.map((c) => c.content);
        const targetStudents = data.targetStudents.map((c) => c.content);
        try {
            await updateCourse(
                course.id,
                {
                    learnedContents,
                    requirements,
                    targetStudents,
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

            <div>
                <div className="text-black font-bold">
                    What will students learn in your course?
                </div>
                <div>
                    You must enter at least 4 learning objectives or outcomes
                    that students can expect to achieve after completing the
                    course.
                </div>
                <LearnedContents control={control} />
            </div>

            <div>
                <div className="text-black font-bold">
                    What are the requirements or prerequisites for taking your
                    course?
                </div>
                <div>
                    List the skills, experience, tools or equipment that
                    students are required to have before taking the course.
                </div>
                <Requirements control={control} />
            </div>

            <div>
                <div className="text-black font-bold">
                    Who is this course intended for?
                </div>
                <div>
                    Write a clear description of the target learners for the
                    course, i.e. people who will find the course content
                    valuable. This will help you attract the right students to
                    join the course.
                </div>
                <TargetStudents control={control} />
            </div>
        </form>
    );
}

function LearnedContents({ control }: { control: Control<FormValues, any> }) {
    const {
        fields: learnedContentsFields,
        append: appendLearnedContent,
        remove: removeLearnedContent,
    } = useFieldArray({
        control,
        name: "learnedContents",
    });

    return (
        <>
            <div className="flex flex-col gap-3 py-5">
                {learnedContentsFields.map((field, index) => (
                    <Controller
                        key={field.id}
                        name={`learnedContents.${index}.content`}
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Field empty.",
                            },
                            maxLength: {
                                value: 150,
                                message: "Maximum 150 characters.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div className="relative">
                                <Input
                                    {...field}
                                    placeholder="Example: Learn how to become jobless with ReactJS"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    labelProps={{
                                        className: "hidden",
                                    }}
                                    className={`bg-white text-gray-900 !border ${
                                        fieldState.error
                                            ? "!border-error"
                                            : "!border-gray-900"
                                    }  placeholder:text-gray-500 placeholder:opacity-100`}
                                />
                                <button
                                    className="absolute right-2 cursor-pointer top-3"
                                    type="button"
                                    disabled={
                                        learnedContentsFields.length === 1
                                    }
                                    onClick={() => removeLearnedContent(index)}
                                >
                                    <FaTrash size={16} />
                                </button>
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
                ))}
            </div>
            {learnedContentsFields.length < MAX_LEARNED_CONTENTS && (
                <button
                    type="button"
                    className="bg-white text-primary flex gap-3 items-center py-2"
                    onClick={() => {
                        appendLearnedContent({ content: "" });
                    }}
                >
                    <IoAddOutline size={26} />
                    Add a content
                </button>
            )}
        </>
    );
}

function Requirements({ control }: { control: Control<FormValues, any> }) {
    const {
        fields: requirementsFields,
        append: appendRequirement,
        remove: removeRequirement,
    } = useFieldArray({
        control,
        name: "requirements",
    });

    return (
        <>
            <div className="flex flex-col gap-3 py-5">
                {requirementsFields.map((field, index) => (
                    <Controller
                        key={field.id}
                        name={`requirements.${index}.content`}
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Field empty.",
                            },
                            maxLength: {
                                value: 150,
                                message: "Maximum 150 characters.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div className="relative">
                                <Input
                                    {...field}
                                    placeholder="Example: No programming experience required"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    labelProps={{
                                        className: "hidden",
                                    }}
                                    className={`bg-white text-gray-900 !border ${
                                        fieldState.error
                                            ? "!border-error"
                                            : "!border-gray-900"
                                    }  placeholder:text-gray-500 placeholder:opacity-100`}
                                />
                                <button
                                    className="absolute right-2 cursor-pointer top-3"
                                    type="button"
                                    disabled={requirementsFields.length === 1}
                                    onClick={() => removeRequirement(index)}
                                >
                                    <FaTrash size={16} />
                                </button>
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
                ))}
            </div>
            {requirementsFields.length < MAX_REQUIREMENTS && (
                <button
                    type="button"
                    className="bg-white text-primary flex gap-3 items-center py-2"
                    onClick={() => {
                        appendRequirement({ content: "" });
                    }}
                >
                    <IoAddOutline size={26} />
                    Add a content
                </button>
            )}
        </>
    );
}

function TargetStudents({ control }: { control: Control<FormValues, any> }) {
    const {
        fields: targetStudentsFields,
        append: appendTargetStudent,
        remove: removeTargetStudent,
    } = useFieldArray({
        control,
        name: "targetStudents",
    });

    return (
        <>
            <div className="flex flex-col gap-3 py-5">
                {targetStudentsFields.map((field, index) => (
                    <Controller
                        key={field.id}
                        name={`targetStudents.${index}.content`}
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: "Field empty.",
                            },
                            maxLength: {
                                value: 150,
                                message: "Maximum 150 characters.",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <div className="relative">
                                <Input
                                    {...field}
                                    placeholder="Example: For those who want to learn ReactJS and became jobless"
                                    crossOrigin={undefined}
                                    error={!!fieldState.error}
                                    labelProps={{
                                        className: "hidden",
                                    }}
                                    className={`bg-white text-gray-900 !border ${
                                        fieldState.error
                                            ? "!border-error"
                                            : "!border-gray-900"
                                    }  placeholder:text-gray-500 placeholder:opacity-100`}
                                />
                                <button
                                    className="absolute right-2 cursor-pointer top-3"
                                    type="button"
                                    disabled={targetStudentsFields.length === 1}
                                    onClick={() => removeTargetStudent(index)}
                                >
                                    <FaTrash size={16} />
                                </button>
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
                ))}
            </div>
            {targetStudentsFields.length < MAX_TARGET_STUDENTS && (
                <button
                    type="button"
                    className="bg-white text-primary flex gap-3 items-center py-2"
                    onClick={() => {
                        appendTargetStudent({ content: "" });
                    }}
                >
                    <IoAddOutline size={26} />
                    Add a content
                </button>
            )}
        </>
    );
}
