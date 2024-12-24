"use client";

import { useEffect, useState } from "react";
import { Session } from "next-auth";
import {
    Button,
    Checkbox,
    Dialog,
    DialogBody,
    DialogHeader,
    Rating as RatingComponent,
} from "@material-tailwind/react";
import { IoIosStar } from "react-icons/io";
import { GoDotFill } from "react-icons/go";

import { Loader } from "@/components/common";
import { CourseDetail } from "@/libs/types/course";
import { Rating } from "@/libs/types/rating";
import { getRatingsByCourse } from "@/services/api/rating";
import RatingItem from "./RatingItem";

const limit = 15;

export default function RatingsModal({
    course,
    open,
    close,
    session,
}: {
    course: CourseDetail;
    open: boolean;
    close: () => void;
    session: Session | null;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [ratingFilter, setRatingFilter] = useState<number>();
    const [ratings, setRatings] = useState<Rating[]>([]);
    const [total, setTotal] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);

    const getMoreRatings = async () => {
        const result = await getRatingsByCourse(
            {
                courseId: course.id,
                skip: ratings.length,
                limit,
                star: ratingFilter,
            },
            session?.accessToken
        );

        if (result) {
            setRatings([...ratings, ...result.items]);
            setEnd(result.end);
            setTotal(result.total);
        }
    };

    useEffect(() => {
        if (!open) return;

        (async () => {
            setIsLoading(true);

            const result = await getRatingsByCourse(
                {
                    courseId: course.id,
                    limit,
                    star: ratingFilter,
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
    }, [course.id, ratingFilter, open]);

    return (
        <Dialog open={open} handler={close} size="lg">
            <DialogHeader className="flex justify-between flex-nowrap">
                <div className="flex gap-3 items-center flex-wrap">
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
                <button onClick={close} className="text-black font-semibold">
                    &#10539;
                </button>
            </DialogHeader>
            <DialogBody className="h-screen overflow-scroll pb-52">
                <div className="md:flex gap-10">
                    <RatingFilter
                        rating={ratingFilter}
                        onRatingChange={setRatingFilter}
                        disabled={isLoading}
                    />
                    <div className="md:flex-grow">
                        {isLoading ? (
                            <div className="flex justify-center">
                                <Loader />
                            </div>
                        ) : (
                            <RatingList
                                ratings={ratings}
                                end={end}
                                next={getMoreRatings}
                            />
                        )}
                    </div>
                </div>
            </DialogBody>
        </Dialog>
    );
}

function RatingList({
    ratings,
    end,
    next,
}: {
    ratings: Rating[];
    end: boolean;
    next: () => Promise<void>;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <>
            {ratings.length === 0 ? (
                <div className="flex justify-center text-gray-700 font-bold">
                    No ratings
                </div>
            ) : (
                <div className="flex flex-col gap-3">
                    {ratings.map((rating) => (
                        <RatingItem key={rating.id} rating={rating} border />
                    ))}
                </div>
            )}
            {!end && (
                <div className="flex items-center justify-center mt-5">
                    <Button
                        loading={isLoading}
                        className="bg-transparent text-gray-900 border border-gray-900"
                        onClick={async () => {
                            setIsLoading(true);
                            await next();
                            setIsLoading(false);
                        }}
                    >
                        Load more
                    </Button>
                </div>
            )}
        </>
    );
}

function RatingFilter({
    rating,
    onRatingChange,
    disabled,
}: {
    rating?: number;
    onRatingChange: (rating?: number) => void;
    disabled?: boolean;
}) {
    return (
        <div className="flex flex-col">
            <div className="flex items-center text-blue-gray-500">
                <Checkbox
                    id="rating-5"
                    checked={rating === 5}
                    onChange={(e) =>
                        onRatingChange(e.target.checked ? 5 : undefined)
                    }
                    disabled={disabled}
                    crossOrigin={undefined}
                />
                <label htmlFor="rating-5">
                    <RatingComponent value={5} readonly />
                </label>
            </div>
            <div className="flex items-center text-blue-gray-500">
                <Checkbox
                    id="rating-4"
                    checked={rating === 4}
                    onChange={(e) =>
                        onRatingChange(e.target.checked ? 4 : undefined)
                    }
                    disabled={disabled}
                    crossOrigin={undefined}
                />
                <label htmlFor="rating-4">
                    <RatingComponent value={4} readonly />
                </label>
            </div>
            <div className="flex items-center text-blue-gray-500">
                <Checkbox
                    id="rating-3"
                    checked={rating === 3}
                    onChange={(e) =>
                        onRatingChange(e.target.checked ? 3 : undefined)
                    }
                    disabled={disabled}
                    crossOrigin={undefined}
                />
                <label htmlFor="rating-3">
                    <RatingComponent value={3} readonly />
                </label>
            </div>
            <div className="flex items-center text-blue-gray-500">
                <Checkbox
                    id="rating-2"
                    checked={rating === 2}
                    onChange={(e) =>
                        onRatingChange(e.target.checked ? 2 : undefined)
                    }
                    disabled={disabled}
                    crossOrigin={undefined}
                />
                <label htmlFor="rating-2">
                    <RatingComponent value={2} readonly />
                </label>
            </div>
            <div className="flex items-center text-blue-gray-500">
                <Checkbox
                    id="rating-1"
                    checked={rating === 1}
                    onChange={(e) =>
                        onRatingChange(e.target.checked ? 1 : undefined)
                    }
                    disabled={disabled}
                    crossOrigin={undefined}
                />
                <label htmlFor="rating-1">
                    <RatingComponent value={1} readonly />
                </label>
            </div>
        </div>
    );
}
