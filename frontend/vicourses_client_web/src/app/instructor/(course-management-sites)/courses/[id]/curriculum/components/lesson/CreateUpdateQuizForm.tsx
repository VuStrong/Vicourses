"use client";

import { Quiz } from "@/libs/types/lesson";
import {
    Button,
    Checkbox,
    Input,
    Tooltip,
    Typography,
} from "@material-tailwind/react";
import { useState } from "react";
import {
    Controller,
    SubmitHandler,
    useFieldArray,
    useForm,
} from "react-hook-form";
import { FaPlus, FaTrash } from "react-icons/fa";

export type QuizFormValues = {
    title: string;
    answers: {
        title: string;
        isCorrect: boolean;
        explanation: string;
    }[];
};

const defaultAnswers = [
    {
        title: "",
        isCorrect: false,
        explanation: "",
    },
    {
        title: "",
        isCorrect: false,
        explanation: "",
    },
];

export default function CreateUpdateQuizForm({
    initialQuizData,
    submit,
    cancel,
}: {
    initialQuizData?: Quiz;
    submit: (formData: QuizFormValues) => Promise<boolean>;
    cancel: () => void;
}) {
    const [isCreating, setIsCreating] = useState<boolean>(false);

    const { handleSubmit, control, reset } = useForm<QuizFormValues>({
        defaultValues: {
            title: initialQuizData?.title || "",
            answers: initialQuizData
                ? initialQuizData.answers.map((a) => ({
                      title: a.title,
                      isCorrect: a.isCorrect,
                      explanation: a.explanation || "",
                  }))
                : defaultAnswers,
        },
    });
    const {
        fields: answersFields,
        append: appendAnswer,
        remove: removeAnswer,
    } = useFieldArray({
        control,
        name: "answers",
    });

    const onSubmit: SubmitHandler<QuizFormValues> = async (data) => {
        if (isCreating) return;

        setIsCreating(true);

        const success = await submit(data);

        if (success) {
            if (initialQuizData) {
                reset(data);
            } else {
                reset();
            }
            cancel();
        }

        setIsCreating(false);
    };

    const handleCancel = () => {
        if (!isCreating) {
            reset();
        }
        cancel();
    };

    return (
        <form className="w-full">
            <Controller
                name="title"
                control={control}
                rules={{
                    required: {
                        value: true,
                        message: "Title must be between 3 and 100 characters.",
                    },
                    minLength: {
                        value: 3,
                        message: "Title must be between 3 and 100 characters.",
                    },
                    maxLength: {
                        value: 100,
                        message: "Title must be between 3 and 100 characters.",
                    },
                }}
                render={({ field, fieldState }) => (
                    <div className="mb-2">
                        <Input
                            label="Title"
                            {...field}
                            disabled={isCreating}
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
            <div className="flex flex-col gap-3">
                {answersFields.map((field, index) => (
                    <div className="md:flex gap-2" key={field.id}>
                        <div className="flex flex-row md:flex-col gap-2 items-center">
                            <Controller
                                name={`answers.${index}.isCorrect`}
                                control={control}
                                render={({ field }) => (
                                    <Tooltip content="Enable this if the answer is correct">
                                        <Checkbox
                                            disabled={isCreating}
                                            checked={field.value}
                                            onChange={(e) =>
                                                field.onChange(e.target.checked)
                                            }
                                            crossOrigin={undefined}
                                        />
                                    </Tooltip>
                                )}
                            />
                            <button
                                className={`right-2 cursor-pointer top-3 ${
                                    answersFields.length === 2 &&
                                    "!cursor-not-allowed"
                                }`}
                                type="button"
                                title="Remove this answer"
                                disabled={
                                    answersFields.length === 2 || isCreating
                                }
                                onClick={() => removeAnswer(index)}
                            >
                                <FaTrash size={16} />
                            </button>
                        </div>
                        <div className="flex-grow flex flex-col gap-3">
                            <Controller
                                name={`answers.${index}.title`}
                                control={control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: "This field is required.",
                                    },
                                    maxLength: {
                                        value: 200,
                                        message: "Maximum 200 characters.",
                                    },
                                }}
                                render={({ field, fieldState }) => (
                                    <div>
                                        <Input
                                            {...field}
                                            disabled={isCreating}
                                            label="Answer"
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
                                name={`answers.${index}.explanation`}
                                control={control}
                                rules={{
                                    minLength: {
                                        value: 5,
                                        message:
                                            "This field must be between 5 and 200 characters.",
                                    },
                                    maxLength: {
                                        value: 200,
                                        message:
                                            "This field must be between 5 and 200 characters.",
                                    },
                                }}
                                render={({ field, fieldState }) => (
                                    <div className="pl-10">
                                        <Input
                                            {...field}
                                            disabled={isCreating}
                                            label="Explanation"
                                            placeholder="Explain why this is or is not the best answer"
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
                        </div>
                    </div>
                ))}
            </div>
            <div className="flex gap-3 flex-col md:flex-row justify-between mt-5">
                <Button
                    variant="text"
                    size="sm"
                    disabled={answersFields.length >= 5 || isCreating}
                    onClick={() =>
                        appendAnswer({
                            title: "",
                            isCorrect: false,
                            explanation: "",
                        })
                    }
                    type="button"
                    className="flex gap-2"
                >
                    <FaPlus size={16} />
                    Add an answer
                </Button>
                <div className="flex gap-3">
                    <Button
                        variant="text"
                        size="sm"
                        onClick={handleCancel}
                        disabled={isCreating}
                        type="button"
                    >
                        Cancel
                    </Button>
                    <Button
                        className="bg-black text-white rounded-none"
                        size="sm"
                        onClick={handleSubmit(onSubmit)}
                        type="submit"
                        loading={isCreating}
                    >
                        Save
                    </Button>
                </div>
            </div>
        </form>
    );
}
