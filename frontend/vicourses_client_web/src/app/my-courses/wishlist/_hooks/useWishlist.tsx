"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { Wishlist } from "@/libs/types/wishlist";
import { getWishlist } from "@/services/api/wishlist";

export default function useWishlist() {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [wishlist, setWishlist] = useState<Wishlist | null>(null);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getWishlist(session.accessToken);

                setWishlist(result);
                setIsLoading(false);
            })();
        }
    }, [status]);

    return {
        isLoading,
        wishlist,
    }
}
