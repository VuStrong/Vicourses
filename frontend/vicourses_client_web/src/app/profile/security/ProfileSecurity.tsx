"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { Loader } from "@/components/common";
import { User } from "@/libs/types/user";
import { getAuthenticatedUser } from "@/services/api/user";
import { Button, Input, Typography } from "@material-tailwind/react";
import { FaPencilAlt } from "react-icons/fa";
import ChangePasswordModal from "./ChangePasswordModal";

export default function ProfileSecurity() {
    const [isChangePasswordModalOpen, setIsChangePasswordModalOpen] = useState<boolean>(false);
    const [user, setUser] = useState<User | null>(null);
    const { data: session } = useSession();

    useEffect(() => {
        (async () => {
            if (session?.accessToken) {
                const result = await getAuthenticatedUser(
                    session.accessToken,
                    "id,email"
                );

                setUser(result);
            }
        })();
    }, [session?.accessToken]);

    if (!user) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <div className="my-5 w-full">
            <ChangePasswordModal 
                isOpen={isChangePasswordModalOpen}
                onClose={() => setIsChangePasswordModalOpen(false)}
                accessToken={session?.accessToken || ""}
            />

            <div className="flex flex-col gap-5">
                <div className="relative w-full max-w-[30rem]">
                    <p className="text-gray-900">Email: </p>
                    <Input
                        label="Email"
                        variant="outlined"
                        disabled
                        value={user.email}
                        className="!border !border-gray-900"
                        labelProps={{
                            className: "!block",
                        }}
                        crossOrigin={undefined}
                    />
                    <Typography
                        variant="small"
                        color="gray"
                        className="mt-2 flex items-center gap-1 font-normal"
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 24 24"
                            fill="currentColor"
                            className="-mt-px h-4 w-4"
                        >
                            <path
                                fillRule="evenodd"
                                d="M2.25 12c0-5.385 4.365-9.75 9.75-9.75s9.75 4.365 9.75 9.75-4.365 9.75-9.75 9.75S2.25 17.385 2.25 12zm8.706-1.442c1.146-.573 2.437.463 2.126 1.706l-.709 2.836.042-.02a.75.75 0 01.67 1.34l-.04.022c-1.147.573-2.438-.463-2.127-1.706l.71-2.836-.042.02a.75.75 0 11-.671-1.34l.041-.022zM12 9a.75.75 0 100-1.5.75.75 0 000 1.5z"
                                clipRule="evenodd"
                            />
                        </svg>
                        Currently you cannot change your email.
                    </Typography>
                </div>
                <div className="w-full max-w-[30rem]">
                    <p className="text-gray-900">Password: </p>
                    <div className="relative">
                        <Input
                            label="Password"
                            value="********"
                            disabled
                            className="!border !border-gray-900"
                            crossOrigin={undefined}
                        />
                        <Button
                            size="sm"
                            onClick={() => setIsChangePasswordModalOpen(true)}
                            className="!absolute right-1 top-1 rounded"
                        >
                            <FaPencilAlt size={16} />
                        </Button>
                    </div>
                </div>
            </div>
        </div>
    );
}
