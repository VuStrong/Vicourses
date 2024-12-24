"use client";

import { useState } from "react";
import { Button } from "@material-tailwind/react";
import { Course } from "@/libs/types/course";
import CreateCouponModal from "./CreateCouponModal";

export default function CreateCouponButton({ course }: { course: Course }) {
    const [modalOpen, setModalOpen] = useState<boolean>(false);

    const getButton = () => {
        if (course.status !== "Published") {
            return (
                <div className="text-black font-semibold">
                    You cannot create a coupon for an unpublished course
                </div>
            );
        }

        if (!course.isPaid) {
            return (
                <div className="text-black font-semibold">
                    You cannot create a coupon for a free course
                </div>
            );
        }

        return (
            <Button
                className="bg-white rounded-none text-gray-900 border border-gray-900"
                onClick={() => setModalOpen(true)}
            >
                Create a coupon
            </Button>
        );
    };

    return (
        <>
            {getButton()}
            <CreateCouponModal
                course={course}
                open={modalOpen}
                onClose={() => setModalOpen(false)}
            />
        </>
    );
}
