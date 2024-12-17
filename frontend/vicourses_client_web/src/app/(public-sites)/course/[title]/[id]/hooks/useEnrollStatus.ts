"use client";

import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { checkEnroll } from "@/services/api/course";

export default function useEnrollStatus(courseId: string) {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [enrolled, setEnrolled] = useState<boolean>(false);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                const isEnrolled = await checkEnroll(
                    courseId,
                    session.accessToken
                );

                setEnrolled(isEnrolled);
                setIsLoading(false);
            })();
        } else if (status === "unauthenticated") {
            setEnrolled(false);
            setIsLoading(false);
        } else {
            setIsLoading(true);
        }
    }, [status]);

    return {
        isLoading,
        enrolled,
    };
}
