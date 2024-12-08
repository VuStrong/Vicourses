"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

export default function Tabs() {
    const pathname = usePathname();

    return (
        <div className="flex flex-nowrap items-center overflow-x-auto text-gray-700 font-semibold">
            <Link
                href="/my-courses"
                className={`py-2 pr-5 hover:opacity-80 border-gray-700 whitespace-nowrap ${
                    pathname === "/my-courses" && "border-b-4"
                }`}
            >
                My courses
            </Link>
            <Link
                href="/my-courses/wishlist"
                className={`py-2 px-5 hover:opacity-80 border-gray-700 ${
                    pathname === "/my-courses/wishlist" && "border-b-4"
                }`}
            >
                Wishlist
            </Link>
        </div>
    );
}
