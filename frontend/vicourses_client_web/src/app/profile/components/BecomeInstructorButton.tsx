"use client";

import { User } from "@/libs/types/user";
import { getAuthenticatedUser, updateRoleToInstructor } from "@/services/api/user";
import {
    Button,
    Dialog,
    DialogBody,
    DialogHeader,
} from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import toast from "react-hot-toast";

export default function BecomeInstructorButton() {
    const [user, setUser] = useState<User | null>(null);
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [missingRequirements, setMissingRequirements] = useState<string[]>();
    const { data: session, status } = useSession();
    const router = useRouter();

    const handleClick = async () => {
        if (isSubmitting) return;

        setIsSubmitting(true);

        try {
            const response = await updateRoleToInstructor(session?.accessToken || "");

            if (!response.success) {
                setMissingRequirements(response.missingRequirements);
                setModalOpen(true);
                setIsSubmitting(false);
                return;
            }

            router.push("/instructor/courses");
            toast.success("You are now an instructor");
        } catch (error: any) {
            toast.error(error.message);            
        }
    };

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                const result = await getAuthenticatedUser(
                    session.accessToken,
                    "id,role"
                );

                setUser(result);
            })();
        }
    }, [status]);

    return (
        <>
            {user && user.role === "student" && (
                <Button
                    onClick={handleClick}
                    loading={isSubmitting}
                    className="border border-gray-200 bg-transparent text-gray-200 mt-5"
                >
                    Become instructor
                </Button>
            )}
            <Dialog open={modalOpen} handler={() => setModalOpen(false)}>
                <DialogHeader className="flex justify-between flex-nowrap">
                    You are almost done
                    <button
                        onClick={() => setModalOpen(false)}
                        className="text-black font-semibold"
                    >
                        &#10539;
                    </button>
                </DialogHeader>
                <DialogBody className="pb-10">
                    <div className="text-black mb-3">
                        Here are some reasons why you can't become an instructor
                        yet:
                    </div>
                    <ul className="list-disc ml-5">
                        {missingRequirements?.map((requirement, index) => (
                            <li key={index}>{requirement}</li>
                        ))}
                    </ul>
                </DialogBody>
            </Dialog>
        </>
    );
}
