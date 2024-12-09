"use client";

import {
    Button,
    Dialog,
    DialogBody,
    DialogFooter,
    DialogHeader,
} from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useMemo, useState } from "react";
import { CourseDetail } from "@/libs/types/course";
import toast from "react-hot-toast";
import { updateCourse } from "@/services/api/course";
import { useRouter } from "next/navigation";

export default function UnpublishCourseButton({
    course,
}: {
    course: CourseDetail;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const { data: session } = useSession();
    const router = useRouter();

    const handleRepublish = async () => {
        if (isLoading) return;

        setIsLoading(true);

        try {
            await updateCourse(
                course.id,
                {
                    status: "Published",
                },
                session?.accessToken || ""
            );

            toast.success("Course published");
            router.refresh();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsLoading(false);
    };

    const handleUnpublish = async () => {
        if (isLoading) return;

        setIsLoading(true);

        try {
            await updateCourse(
                course.id,
                {
                    status: "Unpublished",
                },
                session?.accessToken || ""
            );

            setModalOpen(false);
            toast.success("Course unpublished");
            router.refresh();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsLoading(false);
    };

    const buttonText = useMemo<string>(() => {
        if (!course.isApproved || course.status !== "Unpublished") {
            return "Unpublish";
        }

        return "Republish";
    }, [course]);

    return (
        <>
            <Button
                disabled={!course.isApproved}
                fullWidth
                loading={isLoading}
                className="bg-white rounded-none text-black border border-gray-900 flex justify-center"
                onClick={() => {
                    if (course.status === "Published") {
                        setModalOpen(true);
                    } else {
                        handleRepublish();
                    }
                }}
            >
                {buttonText}
            </Button>
            <Dialog open={modalOpen} handler={() => setModalOpen(false)}>
                <DialogHeader>Unpublish this course?</DialogHeader>
                <DialogBody>
                    Are you sure you want to unpublish this course? New students
                    can't find your course through search. However, current
                    students will still be able to access the course content.
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
                        loading={isLoading}
                        onClick={handleUnpublish}
                    >
                        <span>Confirm</span>
                    </Button>
                </DialogFooter>
            </Dialog>
        </>
    );
}
