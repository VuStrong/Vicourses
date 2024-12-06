"use client";

import { v4 as uuidv4 } from "uuid";
import { useSession } from "next-auth/react";
import { Button, Input, Typography } from "@material-tailwind/react";
import { ChangeEvent, useEffect, useState } from "react";
import toast from "react-hot-toast";
import { User } from "@/libs/types/user";
import { Loader } from "@/components/common";
import { getAuthenticatedUser, updateProfile } from "@/services/api/user";
import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import { getFileExtension } from "@/libs/utils";
import { uploadImage } from "@/services/api/storage";

const validExtensions = ["jpeg", "jpg", "png"];

export default function ProfileThumbnail() {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [error, setError] = useState<string>();
    const [user, setUser] = useState<User | null>(null);
    const [tempThumbnailUrl, setTempThumbnailUrl] = useState<string>();
    const [file, setFile] = useState<File>();
    const { data: session } = useSession();

    const onSubmit = async (e: any) => {
        e.preventDefault();

        if (error || !file) return;

        setIsUpdating(true);
        const fileId = `vicourses-user-photos/${uuidv4()}.${getFileExtension(
            file
        )}`;

        try {
            const uploadResponse = await uploadImage(
                file,
                session?.accessToken || "",
                fileId,
            );

            await updateProfile({
                thumbnailToken: uploadResponse.token,
            }, session?.accessToken || "");

            setFile(undefined);

            toast.success("Photo saved");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    const onImageChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            const fileToUpdate = e.target.files[0];
            const ext = getFileExtension(fileToUpdate);

            if (!validExtensions.includes(ext)) {
                setError(
                    `Invalid image extension. Valid: ${validExtensions.join(
                        ","
                    )}`
                );
                return;
            }

            setFile(fileToUpdate);
            setTempThumbnailUrl(URL.createObjectURL(fileToUpdate));
            setError(undefined);
        }
    };

    useEffect(() => {
        (async () => {
            if (session?.accessToken) {
                const result = await getAuthenticatedUser(
                    session.accessToken,
                    "id,thumbnailUrl"
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
        <form className="my-5 w-full">
            <div className="flex flex-col gap-3 max-w-[18rem]">
                <img
                    className="w-72 h-72 rounded-full object-cover object-center"
                    alt="avatar"
                    src={
                        tempThumbnailUrl
                            ? tempThumbnailUrl
                            : user.thumbnailUrl || DEFAULT_USER_AVATAR_URL
                    }
                />

                <Input
                    type="file"
                    accept=".jpg,.jpeg,.png"
                    crossOrigin={undefined}
                    onChange={onImageChange}
                />

                {error && <Typography color="red">{error}</Typography>}
            </div>
            <div>
                <Button
                    type="submit"
                    disabled={!!error || !file || isUpdating}
                    loading={isUpdating}
                    onClick={onSubmit}
                    className="bg-primary flex justify-center mt-5"
                >
                    Save
                </Button>
            </div>
        </form>
    );
}
