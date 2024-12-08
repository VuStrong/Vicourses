"use client";

import useWishlist from "@/hooks/store/useWishlist";
import { getCourseIdsInWishlist } from "@/services/api/wishlist";
import { useSession } from "next-auth/react";
import { useEffect } from "react";

const GetWishlistHandler = () => {
    const { data: session, status } = useSession();
    const wishlist = useWishlist();

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                const data = await getCourseIdsInWishlist(session.accessToken);
    
                wishlist.setCourseIds(data);
                wishlist.setIsLoading(false);
            })();
        } else if (status === "loading") {
            wishlist.setIsLoading(true);
        } else {
            wishlist.setIsLoading(false);
        }
    }, [status]);

    return null;
};

export default GetWishlistHandler;