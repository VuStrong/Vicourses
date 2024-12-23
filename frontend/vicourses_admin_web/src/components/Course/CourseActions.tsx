import { useState } from "react";
import toast from "react-hot-toast";

import { Course } from "../../types/course";
import axiosInstance from "../../libs/axios";
import Button from "../Button";
import CancelApprovalModal from "./CancelApprovalModal";
import ApproveCourseModal from "./ApproveCourseModal";

export default function CourseActions({
    course,
    onCourseApprovalCanceled,
    onCourseApproved,
}: {
    course: Course;
    onCourseApprovalCanceled?: () => void;
    onCourseApproved?: () => void;
}) {
    const [cancelModalOpen, setCancelModalOpen] = useState<boolean>(false);
    const [approveModalOpen, setApproveModalOpen] = useState<boolean>(false);

    const handleCancelApproval = async (reasons: string[]) => {
        try {
            await axiosInstance.post(
                `/api/cs/v1/courses/${course.id}/cancel-approval`,
                {
                    reasons,
                },
            );

            onCourseApprovalCanceled?.();
            toast.success("Course canceled");
            return true;
        } catch (error: any) {
            toast.error(error.response?.data?.message || "Error");
            return false;
        }
    };

    const handleApproveCourse = async () => {
        try {
            await axiosInstance.post(`/api/cs/v1/courses/${course.id}/approve`);

            onCourseApproved?.();
            toast.success("Course approved");
            return true;
        } catch (error: any) {
            toast.error(error.response?.data?.message || "Error");
            return false;
        }
    };

    return (
        <>
            <div className="flex flex-col md:flex-row gap-3">
                {(course.isApproved || course.status === "WaitingToVerify") && (
                    <div>
                        <Button
                            variant="text"
                            className="border text-red-600 dark:border-gray-600 w-full md:w-fit"
                            onClick={() => setCancelModalOpen(true)}
                        >
                            Cancel approval
                        </Button>
                    </div>
                )}
                {course.status === "WaitingToVerify" && (
                    <div>
                        <Button
                            className="w-full md:w-fit"
                            onClick={() => setApproveModalOpen(true)}
                        >
                            Approve this course
                        </Button>
                    </div>
                )}
            </div>

            <CancelApprovalModal
                open={cancelModalOpen}
                onClose={() => setCancelModalOpen(false)}
                submit={handleCancelApproval}
            />

            <ApproveCourseModal
                open={approveModalOpen}
                onClose={() => setApproveModalOpen(false)}
                submit={handleApproveCourse}
            />
        </>
    );
}
