"use client";

import { useState } from "react";
import { IoMdTrash, IoMdClose, IoMdCheckmark } from "react-icons/io";
import { IoPencil } from "react-icons/io5";

import { Quiz } from "@/libs/types/lesson";

export default function LessonQuizItem({
    quiz,
    deleteFn,
    onClickEdit,
}: {
    quiz: Quiz;
    deleteFn: () => Promise<void>;
    onClickEdit: () => void;
}) {
    const [openDeleteConfirm, setOpenDeleteConfirm] = useState<boolean>(false);
    const [isDeleting, setIsDeleting] = useState<boolean>(false);

    const handleRemoveQuiz = async () => {
        if (isDeleting) return;

        setIsDeleting(true);

        await deleteFn();

        setIsDeleting(false);
        setOpenDeleteConfirm(false);
    };

    return (
        <div className="flex justify-between items-center">
            {openDeleteConfirm && (
                <div className="flex gap-3 justify-end w-full">
                    <div className="text-black font-semibold">
                        Remove this quiz?
                    </div>

                    <button onClick={() => setOpenDeleteConfirm(false)}>
                        <IoMdClose size={16} />
                    </button>
                    <button onClick={handleRemoveQuiz}>
                        <IoMdCheckmark size={16} />
                    </button>
                </div>
            )}

            {!openDeleteConfirm && (
                <>
                    <div className="text-sm flex gap-2">
                        <span className="text-black font-semibold">
                            {quiz.number}.
                        </span>
                        <span className="text-gray-900">{quiz.title}</span>
                    </div>
                    <div className="flex gap-2">
                        <button
                            onClick={onClickEdit}
                        >
                            <IoPencil size={18} />
                        </button>
                        <button onClick={() => setOpenDeleteConfirm(true)}>
                            <IoMdTrash size={18} />
                        </button>
                    </div>
                </>
            )}
        </div>
    );
}
