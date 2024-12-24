"use client";

import { useState } from "react";
import { Button } from "@material-tailwind/react";
import { Rating } from "@/libs/types/rating";
import RatingItem from "./RatingItem";

export default function RatingList({
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
                <div className="flex justify-center text-gray-900">
                    No rating found
                </div>
            ) : (
                <div className="flex flex-col gap-3">
                    {ratings.map((rating) => (
                        <RatingItem key={rating.id} initialRating={rating} />
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
