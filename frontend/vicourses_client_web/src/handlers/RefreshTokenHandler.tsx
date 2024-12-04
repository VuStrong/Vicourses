"use client";

import { signOut, useSession } from "next-auth/react";
import { Dispatch, SetStateAction, useEffect } from "react";

const RefreshTokenHandler = ({
    setInterval,
}: {
    setInterval: Dispatch<SetStateAction<number>>;
}) => {
    const { data: session } = useSession();

    useEffect(() => {
        if (session?.error === "RefreshAccessTokenError") {
            signOut();
        }

        if (session) {
            // get millisecond offset from now to expire time
            // (plus 60s to ensure refresh)
            const offsetTime =
                new Date(session.accessTokenExpiry).getTime() +
                60 * 1000 -
                Date.now();
            const timeToRefresh = Math.round(offsetTime / 1000);

            setInterval(timeToRefresh > 0 ? timeToRefresh : 0);
        }
    }, [session]);

    return null;
};

export default RefreshTokenHandler;