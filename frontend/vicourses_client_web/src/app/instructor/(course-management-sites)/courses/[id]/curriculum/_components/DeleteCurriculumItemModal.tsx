"use client";

import {
    Button,
    Dialog,
    DialogBody,
    DialogFooter,
    DialogHeader,
} from "@material-tailwind/react";
import { Session } from "next-auth";
import { useState } from "react";
import toast from "react-hot-toast";

import { deleteSection } from "@/services/api/course-section";
import { deleteLesson } from "@/services/api/course-lesson";
import { CurriculumItem } from "../_lib/types";
import useDeleteCurriculumItemModal from "../_hooks/useDeleteCurriculumItemModal";

export default function DeleteCurriculumItemModal({
    session,
    onItemDeleted,
}: {
    session: Session | null;
    onItemDeleted?: (item: CurriculumItem) => void;
}) {
    const { isOpen, close, item } = useDeleteCurriculumItemModal();
    const [isDeleting, setIsDeleting] = useState<boolean>(false);

    const handleDelete = async () => {
        if (isDeleting || !item) return;

        setIsDeleting(true);

        try {
            if (item.type === "Section") {
                await deleteSection(item.id, session?.accessToken || "");
            } else {
                await deleteLesson(item.id, session?.accessToken || "");
            }
            
            onItemDeleted?.(item);
            close();
            toast.success("Deleted");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsDeleting(false);
    };

    return (
        <Dialog open={isOpen} handler={close}>
            <DialogHeader>Delete this item?</DialogHeader>
            <DialogBody>
                Are you sure you want to delete this item? This action is
                permanent and cannot be undone.
            </DialogBody>
            <DialogFooter>
                <Button variant="text" onClick={close} className="mr-1">
                    <span>Cancel</span>
                </Button>
                <Button
                    className="bg-black text-white"
                    loading={isDeleting}
                    onClick={handleDelete}
                >
                    <span>Confirm</span>
                </Button>
            </DialogFooter>
        </Dialog>
    );
}
