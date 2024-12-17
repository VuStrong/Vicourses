"use client";

import { useState } from "react";
import { useSession } from "next-auth/react";
import { usePathname, useRouter } from "next/navigation";
import toast from "react-hot-toast";
import { Button } from "@material-tailwind/react";

import { CourseDetail } from "@/libs/types/course";
import { enrollToFreeCourse } from "@/services/api/course";

export default function EnrollButton({ course }: { course: CourseDetail }) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const { data: session, status } = useSession();
    const router = useRouter();
    const pathname = usePathname();

    const handleEnroll = async () => {
        if (status === "unauthenticated") {
            router.push(`/login?callbackUrl=${pathname}`);
            return;
        }

        if (isLoading || !session) return;
        setIsLoading(true);

        try {
            await enrollToFreeCourse(course.id, session.accessToken);

            router.push(`/learn/course/${course.id}`);
            return;
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsLoading(false);
    }

    return (
        <Button
            type="button"
            className="flex justify-center items-center rounded-none"
            loading={isLoading}
            fullWidth
            onClick={handleEnroll}
        >
            Enroll now
        </Button>
    );
}
