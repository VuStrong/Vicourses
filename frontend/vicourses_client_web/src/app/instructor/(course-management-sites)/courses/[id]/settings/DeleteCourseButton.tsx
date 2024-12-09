"use client";

import { deleteCourse } from "@/services/api/course";
import {
    Button,
    Dialog,
    DialogBody,
    DialogFooter,
    DialogHeader,
} from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useState } from "react";
import toast from "react-hot-toast";

export default function DeleteCourseButton({
    courseId,
    disabled,
}: {
    courseId: string;
    disabled: boolean;
}) {
    const [isDeleting, setIsDeleting] = useState<boolean>(false);
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const { data: session } = useSession();
    const router = useRouter();

    const handleDelete = async () => {
        if (isDeleting) return;

        setIsDeleting(true);
        
        try {
            await deleteCourse(courseId, session?.accessToken || "");

            toast.success("Course deleted");
            router.replace("/instructor/courses");
        } catch (error: any) {
            toast.error(error.message);    
        }

        setIsDeleting(false);
    };

    return (
        <>
            <Button
                disabled={disabled}
                fullWidth
                className="bg-white rounded-none text-black border border-gray-900"
                onClick={() => setModalOpen(true)}
            >
                Delete
            </Button>
            <Dialog open={modalOpen} handler={() => setModalOpen(false)}>
                <DialogHeader>Delete this course?</DialogHeader>
                <DialogBody>
                    Are you sure you want to delete this course? This action is
                    permanent and cannot be undone.
                </DialogBody>
                <DialogFooter>
                    <Button
                        variant="text"
                        onClick={() => setModalOpen(false)}
                        className="mr-1"
                    >
                        <span>Cancel</span>
                    </Button>
                    <Button
                        className="bg-primary"
                        loading={isDeleting}
                        onClick={handleDelete}
                    >
                        <span>Confirm</span>
                    </Button>
                </DialogFooter>
            </Dialog>
        </>
    );
}
