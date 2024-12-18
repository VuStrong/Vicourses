"use client";

import { useState } from "react";
import Link from "next/link";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { CiHeart } from "react-icons/ci";
import { BiMenuAltLeft, BiSearch } from "react-icons/bi";

import { SearchBar } from "@/components/common";
import UserMenu from "@/components/common/UserMenu";
import HeaderCategoriesMenu from "./HeaderCategoriesMenu";
import MobileSidebarDrawer from "./MobileSidebarDrawer";

export default function PublicSitesHeader() {
    return (
        <>
            <MobileHeader />
            <DesktopHeader />
        </>
    );
}

function MobileHeader() {
    const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
    const [openSearch, setOpenSearch] = useState<boolean>(false);
    const router = useRouter();

    return (
        <>
            <MobileSidebarDrawer
                open={isDrawerOpen}
                onClose={() => setIsDrawerOpen(false)}
            />

            <header
                className={`md:hidden flex items-center justify-between flex-wrap gap-5 py-2 bg-white shadow-lg`}
            >
                {openSearch && (
                    <>
                        <div className="flex-grow pl-2">
                            <SearchBar
                                onSubmit={(value) => {
                                    router.push(`/search?q=${value}`);
                                }}
                            />
                        </div>
                        <button
                            onClick={() => setOpenSearch(false)}
                            className="text-black font-semibold pr-2"
                        >
                            Close
                        </button>
                    </>
                )}

                {!openSearch && (
                    <>
                        <button
                            type="button"
                            className="p-2 rounded-lg focus:outline-none focus:ring-2"
                            onClick={() => setIsDrawerOpen(true)}
                        >
                            <BiMenuAltLeft
                                size={28}
                                className="text-gray-900"
                            />
                        </button>

                        <div></div>

                        <Link href="/" className="w-[60px] -translate-x-1/3">
                            <Image
                                src="/img/logo-transparent.png"
                                width={80}
                                height={40}
                                alt="Vicourses"
                            />
                        </Link>

                        <div className="flex items-center gap-2">
                            <button
                                type="button"
                                className="py-2 rounded-lg focus:outline-none focus:ring-2"
                                onClick={() => setOpenSearch(true)}
                            >
                                <BiSearch size={28} className="text-gray-900" />
                            </button>

                            <UserMenu />
                        </div>
                    </>
                )}
            </header>
        </>
    );
}

function DesktopHeader() {
    const router = useRouter();

    return (
        <header
            className={`hidden md:flex items-center flex-wrap gap-5 py-2 px-5 bg-white shadow-lg`}
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
