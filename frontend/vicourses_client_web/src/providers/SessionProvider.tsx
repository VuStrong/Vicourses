"use client";

import RefreshTokenHandler from "@/handlers/RefreshTokenHandler";
import { SessionProvider } from "next-auth/react";
import { useState } from "react";

export default function SessionProviderWrapper({
    children,
    session,
}: {
    children: React.ReactNode;
    session?: any;
}) {
    const [interval, setInterval] = useState<number>(0);

    return (
        <SessionProvider session={session} refetchInterval={interval}>
            {children}
            <RefreshTokenHandler setInterval={setInterval} />
        </SessionProvider>
    );
}