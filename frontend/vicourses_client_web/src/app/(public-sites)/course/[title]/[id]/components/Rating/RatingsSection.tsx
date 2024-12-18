"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { IoIosStar } from "react-icons/io";
import { GoDotFill } from "react-icons/go";
import { Button } from "@material-tailwind/react";

import { Loader } from "@/components/common";
import { Rating } from "@/libs/types/rating";
import { CourseDetail } from "@/libs/types/course";
import { getRatingsByCourse } from "@/services/api/rating";
import RatingItem from "./RatingItem";
import RatingsModal from "./RatingsModal";
import UserRating from "./UserRating";

export default function RatingsSection({ course }: { course: CourseDetail }) {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [ratings, setRatings] = useState<Rating[]>([]);
    const [total, setTotal] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "loading") return;

        (async () => {
            setIsLoading(true);

            const result = await getRatingsByCourse(
                {
                    courseId: course.id,
                    limit: 4,
                },
                session?.accessToken
            );

            if (result) {
                setRatings(result.items);
                setEnd(result.end);
                setTotal(result.total);
            }

            setIsLoading(false);
        })();
    }, [course.id, status]);

    if (isLoading) {
        return (
            <div className="flex justify-center mt-10">
                <Loader />
            </div>
        );
    }

    return (
        <>
            <section className="mt-7">
                <div className="flex gap-3 items-center flex-wrap mb-4">
                    <div className="text-yellow-700">
                        <IoIosStar size={20} />
                    </div>
                    <span className="text-black font-bold text-2xl">
                        {course.rating} course rating
                    </span>
                    <div>
                        <GoDotFill size={14} />
                    </div>
                    <span className="text-black font-bold text-2xl">
                        {total} rating
                    </span>
                </div>

                <UserRating course={course} />

                {ratings.length === 0 ? (
                    <div className="text-gray-700 font-bold text-center">
                        No ratings
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-3 mb-3">
                        {ratings.map((rating) => (
                            <RatingItem key={rating.id} rating={rating} />
                        ))}
                    </div>
                )}

                {!end && (
                    <Button
                        variant="text"
                        className="border border-gray-900 rounded-none"
                        onClick={() => setModalOpen(true)}
                    >
                        Show all ratings
                    </Button>
                )}
            </section>
            <RatingsModal
                course={course}
                open={modalOpen}
                close={() => setModalOpen(false)}
                session={session}
            />
        </>
    );
}
