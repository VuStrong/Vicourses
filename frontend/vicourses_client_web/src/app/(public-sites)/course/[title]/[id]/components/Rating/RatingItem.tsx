"use client";

import { Avatar, Rating as RatingComponent } from "@material-tailwind/react";

import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import { Rating } from "@/libs/types/rating";

export default function RatingItem({
    rating,
    border,
}: {
    rating: Rating;
    border?: boolean;
}) {
    return (
        <div className={`py-2 ${border && "border-b border-gray-300"}`}>
            <div className="flex gap-3">
                <Avatar
                    src={rating.user?.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                    alt={rating.user?.name}
                />
                <div className="flex-grow">
                    <div className="text-black font-bold line-clamp-1">
                        {rating.user?.name}
                    </div>
                    <div className="flex gap-1 flex-wrap items-center">
                        <RatingComponent
                            value={rating.star}
                            readonly
                            className="[&_svg]:w-5 [&_svg]:h-5"
                        />
                        <div className="text-gray-600 font-semibold text-sm">
                            {new Date(rating.createdAt).toLocaleDateString()}
                        </div>
                    </div>
                </div>
            </div>
            <div className="text-gray-900 my-5">{rating.feedback}</div>
            {rating.responded && (
                <div className="border-l-2 border-black pl-5">
                    <div className="font-semibold text-gray-700 text-sm">
                        Instructor response -{" "}
                        {new Date(
                            rating.respondedAt || ""
                        ).toLocaleDateString()}
                    </div>
                    <div>{rating.response}</div>
                </div>
            )}
        </div>
    );
}
