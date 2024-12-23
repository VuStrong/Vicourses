import { useState } from "react";
import {
    Controller,
    SubmitHandler,
    useFieldArray,
    useForm,
} from "react-hook-form";
import { FaPlus, FaTrash } from "react-icons/fa";

import Button from "../Button";
import { Input } from "../Forms";
import { Modal, ModalBody, ModalFooter, ModalHeader } from "../Modal";

type FormValues = {
    reasons: {
        content: string;
    }[];
};

export default function CancelApprovalModal({
    open,
    onClose,
    submit,
}: {
    open: boolean;
    onClose: () => void;
    submit: (reasons: string[]) => Promise<boolean>;
}) {
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const { handleSubmit, control, reset } = useForm<FormValues>({
        defaultValues: {
            reasons: [{ content: "" }],
        },
    });
    const { fields, append, remove } = useFieldArray({
        control,
        name: "reasons",
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        setIsSubmitting(true);

        const success = await submit(data.reasons.map((c) => c.content));
        
        if (success) {
            reset();
            onClose();
        }

        setIsSubmitting(false);
    };

    const handleClose = () => {
        if (!isSubmitting) {
            reset();
        }
        onClose();
    };

    return (
        <Modal open={open} onClose={handleClose}>
            <ModalHeader>Cancel course approval</ModalHeader>
            <ModalBody>
                <div className="flex flex-col gap-3">
                    {fields.map((field, index) => (
                        <Controller
                            key={field.id}
                            name={`reasons.${index}.content`}
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: "Field empty.",
                                },
                                maxLength: {
                                    value: 300,
                                    message: "Maximum 300 characters.",
                                },
                            }}
                            render={({ field, fieldState }) => (
                                <div className="relative">
                                    <Input
                                        value={field.value}
                                        onChange={field.onChange}
                                        placeholder="Enter reason"
                                        error={!!fieldState.error}
                                        disabled={isSubmitting}
                                    />
                                    <button
                                        className="absolute right-2 cursor-pointer top-4"
                                        type="button"
                                        disabled={
                                            fields.length === 1 || isSubmitting
                                        }
                                        onClick={() => remove(index)}
                                    >
                                        <FaTrash size={16} />
                                    </button>
                                    {fieldState.error && (
                                        <p className="text-red-600">{fieldState.error.message}</p>
                                    )}
                                </div>
                            )}
                        />
                    ))}

                    <div className="mt-5">
                        <Button
                            variant="text"
                            className="dark:border-gray-600 border"
                            onClick={() => append({ content: "" })}
                            disabled={isSubmitting}
                        >
                            <FaPlus />
                            Add a reason
                        </Button>
                    </div>
                </div>
            </ModalBody>
            <ModalFooter>
                <Button onClick={handleClose} variant="text">
                    Cancel
                </Button>
                <Button onClick={handleSubmit(onSubmit)} loading={isSubmitting}>
                    Confirm
                </Button>
            </ModalFooter>
        </Modal>
    );
}
