"use client";

import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { getAuthenticatedUser } from "@/services/api/user";
import { User } from "@/libs/types/user";

export default function useUser(fields?: string): {
    user: User | null;
    isLoading: boolean;
} {
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                const result = await getAuthenticatedUser(session.accessToken, fields);

                setUser(result);
                setIsLoading(false);
            })();
        } else if (status === "unauthenticated") {
            setUser(null);
            setIsLoading(false);
        } else {
            setIsLoading(true);
        }
    }, [status, session]);

    return {
        user,
        isLoading,
    };
}