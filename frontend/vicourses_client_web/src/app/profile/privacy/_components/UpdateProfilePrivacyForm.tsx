"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { Button, Checkbox } from "@material-tailwind/react";
import toast from "react-hot-toast";
import { Loader } from "@/components/common";
import { getAuthenticatedUser, updateProfile } from "@/services/api/user";

type UserPrivacy = {
    isPublic: boolean;
    enrolledCoursesVisible: boolean;
};

export default function UpdateProfilePrivacyForm() {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [privacy, setPrivacy] = useState<UserPrivacy>();
    const { data: session } = useSession();

    const onSubmit = async (e: any) => {
        e.preventDefault();

        if (!privacy) return;

        setIsUpdating(true);

        try {
            await updateProfile({
                isPublic: privacy.isPublic,
                enrolledCoursesVisible: privacy.enrolledCoursesVisible,
            }, session?.accessToken || "");
            
            toast.success("Saved");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsUpdating(false);
    };

    useEffect(() => {
        (async () => {
            if (session?.accessToken) {
                const result = await getAuthenticatedUser(
                    session.accessToken,
                    "id,isPublic,enrolledCoursesVisible"
                );
                
                if (result) {
                    setPrivacy({
                        isPublic: result.isPublic,
                        enrolledCoursesVisible: result.enrolledCoursesVisible,
                    });
                }
            }
        })();
    }, [session?.accessToken]);

    if (!privacy) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <form className="my-5 w-full">
            <div className="flex flex-col gap-3">
                <Checkbox
                    checked={privacy.isPublic}
                    label="Show your profile to everyone"
                    crossOrigin={undefined}
                    onChange={(value) => {
                        setPrivacy({
                            ...privacy,
                            isPublic: value.target.checked,
                        });
                    }}
                />
                <Checkbox
                    checked={privacy.enrolledCoursesVisible}
                    label="Show your enrolled courses on your profile"
                    crossOrigin={undefined}
                    onChange={(value) => {
                        setPrivacy({
                            ...privacy,
                            enrolledCoursesVisible: value.target.checked,
                        });
                    }}
                />
            </div>
            <div>
                <Button
                    type="submit"
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
