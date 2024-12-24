"use client";

import { Loader } from "@/components/common";
import WishlistedCourseCard from "@/components/course/WishlistedCourseCard";
import useWishlist from "./_hooks/useWishlist";

export default function WishlistPage() {
    const { isLoading, wishlist } = useWishlist();

    if (isLoading) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    if (!wishlist?.courses || wishlist.courses.length === 0) {
        return (
            <div className="flex justify-center text-black font-semibold">
                Your wishlist is empty
            </div>
        );
    }

    return (
        <div className="flex flex-col gap-5">
            {wishlist.courses.map(course => (
                <WishlistedCourseCard key={course.id} course={course} />
            ))}
        </div>
    );
}
