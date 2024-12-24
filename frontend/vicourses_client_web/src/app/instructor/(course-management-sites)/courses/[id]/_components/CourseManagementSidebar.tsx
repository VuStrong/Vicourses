"use client";

import {
    Button,
    Card,
    Dialog,
    DialogBody,
    DialogHeader,
    List,
    ListItem,
} from "@material-tailwind/react";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { useState } from "react";
import { useSession } from "next-auth/react";
import toast from "react-hot-toast";

import { Course, CourseCheckResponse } from "@/libs/types/course";
import { checkCourse, updateCourse } from "@/services/api/course";

export default function CourseManagementSidebar({
    course,
    onCLickItem,
}: {
    course: Course;
    onCLickItem?: () => void;
}) {
    const pathname = usePathname();

    return (
        <Card className="min-h-screen h-full w-full max-w-[20rem] p-4 bg-white shadow-none rounded-none border-none">
            <List className="text-gray-800">
                <div className="text-black font-bold p-3 text-lg">Basics</div>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${course.id}`}
                    className={`${
                        pathname.endsWith(course.id) &&
                        "border-l-4 border-gray-800"
                    }`}
                >
                    <ListItem>Overview</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${course.id}/goals`}
                    className={`${
                        pathname.endsWith("goals") &&
                        "border-l-4 border-gray-800"
                    }`}
                >
                    <ListItem>Goals</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${course.id}/pricing`}
                    className={`${
                        pathname.endsWith("pricing") &&
                        "border-l-4 border-gray-800"
                    }`}
                >
                    <ListItem>Pricing</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${course.id}/promotions`}
                    className={`${
                        pathname.endsWith("promotions") &&
                        "border-l-4 border-gray-800"
                    }`}
                >
                    <ListItem>Promotions</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${course.id}/curriculum`}
                    className={`${
                        pathname.endsWith("curriculum") &&
                        "border-l-4 border-gray-800"
                    }`}
                >
                    <ListItem>Curriculum</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${course.id}/settings`}
                    className={`${
                        pathname.endsWith("settings") &&
                        "border-l-4 border-gray-800"
                    }`}
                >
                    <ListItem>Settings</ListItem>
                </Link>
            </List>
            <SubmitCourseButton course={course} />
        </Card>
    );
}

function SubmitCourseButton({ course }: { course: Course }) {
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [result, setResult] = useState<CourseCheckResponse | null>(null);
    const { data: session } = useSession();
    const router = useRouter();

    const onSubmit = async () => {
        if (isSubmitting || !session?.accessToken) return;

        setIsSubmitting(true);

        const checkResult = await checkCourse(course.id);

        if (!checkResult) return;

        setResult(checkResult);

        if (!checkResult.isValid) {
            setModalOpen(true);
            setIsSubmitting(false);
            return;
        }

        try {
            await updateCourse(
                course.id,
                {
                    status: "WaitingToVerify",
                },
                session.accessToken
            );

            toast.success("Your couse has been submitted");
            router.refresh();
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsSubmitting(false);
    };

    const getButton = () => {
        if (course.isApproved) {
            return null;
        }
        if (course.status === "Unpublished") {
            return (
                <Button
                    className="bg-primary rounded-none mt-5"
                    onClick={onSubmit}
                    type="button"
                    loading={isSubmitting}
                >
                    Submit for verification
                </Button>
            );
        }
        if (course.status === "WaitingToVerify") {
            return (
                <Button
                    className="bg-white text-gray-900 border border-gray-900 rounded-none mt-5"
                    disabled
                >
                    Verifying
                </Button>
            );
        }
    };

    return (
        <>
            {getButton()}
            <Dialog open={modalOpen} handler={() => setModalOpen(false)}>
                <DialogHeader className="flex justify-between flex-nowrap">
                    Why can't I submit for verification?
                    <button
                        onClick={() => setModalOpen(false)}
                        className="text-black font-semibold"
                    >
                        &#10539;
                    </button>
                </DialogHeader>
                <DialogBody className="pb-10">
                    <div className="text-black mb-3">
                        Here are some reasons why you can't submit your course
                        for verification:
                    </div>
                    <ul className="list-disc ml-5">
                        {result?.missingRequirements.map((content, index) => (
                            <li key={index}>{content}</li>
                        ))}
                    </ul>
                </DialogBody>
            </Dialog>
        </>
    );
}
