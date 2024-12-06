"use client";

import Link from "next/link";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { CiHeart } from "react-icons/ci";
import { SearchBar } from "@/components/common";
import UserMenu from "@/components/common/UserMenu";
import HeaderCategoriesMenu from "./HeaderCategoriesMenu";

export default function PublicSitesHeader() {
    const router = useRouter();

    return (
        <header
            className={`flex items-center flex-wrap gap-5 py-2 px-5 bg-white shadow-lg`}
        >
            <Link href="/" className="w-[60px]">
                <Image
                    src="/img/logo-transparent.png"
                    width={100}
                    height={50}
                    alt="Vicourses"
                />
            </Link>

            <HeaderCategoriesMenu />

            <div className="flex-grow">
                <SearchBar
                    onSubmit={(value) => {
                        router.push(`/search?q=${value}`);
                    }}
                />
            </div>

            <Link href="/my-courses/wishlist" title="Wishlist">
                <CiHeart size={32} />
            </Link>

            <UserMenu />
        </header>
    );
}
