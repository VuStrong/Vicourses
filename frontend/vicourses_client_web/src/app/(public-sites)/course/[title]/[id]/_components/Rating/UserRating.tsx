"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import {
    Button,
    Menu,
    MenuHandler,
    MenuItem,
    MenuList,
    Rating as RatingComponent,
} from "@material-tailwind/react";
import toast from "react-hot-toast";
import { HiDotsHorizontal } from "react-icons/hi";

import { Loader } from "@/components/common";
import { CourseDetail } from "@/libs/types/course";
import { Rating } from "@/libs/types/rating";
import { deleteRating, getUserRating } from "@/services/api/rating";
import useEnrollStatus from "../../_hooks/useEnrollStatus";
import RatingItem from "./RatingItem";
import RatingForm from "./RatingForm";

export default function UserRating({ course }: { course: CourseDetail }) {
    const isCheckingEnrolled = useEnrollStatus((state) => state.isLoading);
    const enrolled = useEnrollStatus((state) => state.enrolled);

    const [userRating, setUserRating] = useState<Rating | null>(null);
    const [isLoadingRating, setIsLoadingRating] = useState<boolean>(true);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status !== "authenticated" || isCheckingEnrolled || !enrolled)
            return;

        (async () => {
            setIsLoadingRating(true);

            const result = await getUserRating(session.accessToken, course.id);

            setUserRating(result);
            setIsLoadingRating(false);
        })();
    }, [status, course.id, isCheckingEnrolled]);

    const handleDeleteRating = async () => {
        if (!userRating || status !== "authenticated") return;

        try {
            await deleteRating(userRating.id, session.accessToken);
            
            setUserRating(null);
        } catch (error: any) {
            toast.error(error.message);
        }
    }

    if (!enrolled) {
        return null;
    }

    if (isLoadingRating) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <div className="mb-4 border-b border-gray-600 pb-5">
            {openForm && (
                <RatingForm
                    initialRating={userRating}
                    courseId={course.id}
                    onCancel={() => setOpenForm(false)}
                    session={session}
                    onRatingChanged={(rating) => {
                        setUserRating(rating);
                        setOpenForm(false);
                    }}
                />
            )}

            {!openForm &&
                (userRating ? (
                    <div>
                        <div className="text-gray-900 font-semibold text-xl">
                            Your rating
                        </div>
                        <RatingItem rating={userRating} />
                        <div className="flex gap-2">
                            <Button
                                variant="text"
                                className="text-primary p-0"
                                onClick={() => setOpenForm(true)}
                            >
                                Edit your feedback
                            </Button>
                            <Menu>
                                <MenuHandler>
                                    <button>
                                        <HiDotsHorizontal size={20} />
                                    </button>
                                </MenuHandler>
                                <MenuList>
                                    <MenuItem onClick={handleDeleteRating}>
                                        Delete
                                    </MenuItem>
                                </MenuList>
                            </Menu>
                        </div>
                    </div>
                ) : (
                    <div>
                        <div className="">
                            <RatingComponent
                                value={0}
                                onChange={() => setOpenForm(true)}
                            />
                        </div>
                        <Button
                            variant="text"
                            className="text-primary p-0"
                            onClick={() => setOpenForm(true)}
                        >
                            Write a feedback
                        </Button>
                    </div>
                ))}
        </div>
    );
}
