"use client";

import { useSession } from "next-auth/react";
import toast from "react-hot-toast";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import useWishlist from "@/hooks/store/useWishlist";
import { addToWishlist, removeFromWishlist } from "@/services/api/wishlist";

export default function AddToWishlistButton({
    courseId,
}: {
    courseId: string;
}) {
    const { data: session, status } = useSession();
    const wishlist = useWishlist();
    const added = wishlist.courseIds.has(courseId);

    const handleClick = () => {
        if (status === "unauthenticated") {
            toast("Log in to add to your wishlist");
            return;
        }

        if (wishlist.isLoading) return;

        if (added) {
            removeFromWishlist(courseId, session?.accessToken || "");
            wishlist.removeCourseId(courseId);
        } else {
            addToWishlist(courseId, session?.accessToken || "");
            wishlist.addCourseId(courseId);
        }
    };

    if (wishlist.isLoading) {
        return null;
    }

    return (
        <button
            title={added ? "Remove from wishlist" : "Add to wishlist"}
            className={`text-primary hover:scale-105`}
            onClick={handleClick}
        >
            {added ? <AiFillHeart size={36} /> : <AiOutlineHeart size={36} />}
        </button>
    );
}
