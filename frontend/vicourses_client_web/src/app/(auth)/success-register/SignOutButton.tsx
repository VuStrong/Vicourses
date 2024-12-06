"use client";

import { signOut, useSession } from "next-auth/react";
import { Button } from "@material-tailwind/react";
import { revokeRefreshToken } from "@/services/api/auth";

export default function SignOutButton() {
    const session = useSession();

    const handleSignOut = () => {
        revokeRefreshToken(
            session.data?.user.id ?? "",
            session.data?.refreshToken ?? ""
        ).catch(() => undefined);

        signOut();
    };

    return <Button onClick={handleSignOut} className="bg-transparent text-gray-900">Sign out</Button>;
}
