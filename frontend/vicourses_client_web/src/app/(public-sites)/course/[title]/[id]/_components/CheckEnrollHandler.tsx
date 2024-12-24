"use client"

import { useEffect } from "react";
import { useSession } from "next-auth/react";
import useEnrollStatus from "../_hooks/useEnrollStatus";

export default function CheckEnrollHandler({
    courseId,
}: {
    courseId: string;
}) {
    const check = useEnrollStatus(state => state.check);
    const { data: session, status } = useSession();
    
    useEffect(() => {
        if (status === "loading") return; 
        
        check(courseId, session?.accessToken);
    }, [status, courseId]);

    return null;
}