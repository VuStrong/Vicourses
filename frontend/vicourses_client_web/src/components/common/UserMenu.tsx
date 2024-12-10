"use client";

import { signOut, useSession } from "next-auth/react";
import { usePathname } from "next/navigation";
import Link from "next/link";
import {
    Avatar,
    Menu,
    MenuHandler,
    MenuItem,
    MenuList,
} from "@material-tailwind/react";

import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import { revokeRefreshToken } from "@/services/api/auth";
import useUser from "@/hooks/useUser";

export default function UserMenu() {
    const { user, isLoading } = useUser("id,name,email,thumbnailUrl,role");
    const pathname = usePathname();
    const session = useSession();

    const handleSignOut = () => {
        revokeRefreshToken(
            session.data?.user.id ?? "",
            session.data?.refreshToken ?? ""
        ).catch(() => undefined);

        signOut();
    };

    if (isLoading) {
        return null;
    }

    return user ? (
        <Menu placement="bottom-start">
            <MenuHandler>
                <Avatar
                    variant="circular"
                    className="cursor-pointer"
                    alt={user.name}
                    src={user.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                />
            </MenuHandler>
            <MenuList>
                <Link href="/profile">
                    <MenuItem className="flex items-center gap-2">
                        <Avatar
                            variant="circular"
                            className="cursor-pointer"
                            src={user.thumbnailUrl ?? DEFAULT_USER_AVATAR_URL}
                            alt={user.name}
                        />
                        <div className="max-w-[200px]">
                            <div className="font-bold text-base text-black truncate">
                                {user.name}
                            </div>
                            <div className="text-gray-800 text-sm truncate">
                                {user.email}
                            </div>
                        </div>
                    </MenuItem>
                </Link>

                <hr className="my-3" />
                <Link href={`/my-courses`}>
                    <MenuItem>Study</MenuItem>
                </Link>
                <Link href={`/my-courses/wishlist`}>
                    <MenuItem>Wishlist</MenuItem>
                </Link>
                {user.role === "instructor" && (
                    <Link href={`/instructor/courses`}>
                        <MenuItem>Instructor dashboard</MenuItem>
                    </Link>
                )}

                <hr className="my-3" />
                <Link href={`/user/${user.id}`}>
                    <MenuItem>Public profile</MenuItem>
                </Link>
                <Link href={`/profile`}>
                    <MenuItem>Edit profile</MenuItem>
                </Link>

                <hr className="my-3" />
                <MenuItem onClick={handleSignOut}>Sign out</MenuItem>
            </MenuList>
        </Menu>
    ) : (
        <Link
            href={`/login?callbackUrl=${pathname}`}
            className="border-gray-900 border-2 rounded-md px-4 py-2 font-semibold hover:bg-gray-200 text-gray-900"
        >
            Sign In
        </Link>
    );
}
