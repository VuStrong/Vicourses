"use client";

import { Loader } from "@/components/common";
import { Rating } from "@/libs/types/rating";
import { getInstructorCourses } from "@/services/api/course";
import { getRatingsByInstructor } from "@/services/api/rating";
import { Select, Option, Checkbox, Typography } from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import RatingList from "./RatingList";
import { ratingsFake } from "./fakeData";

const limit = 15;
type CourseOption = {
    value: string;
    label: string;
};
type RatingsFilter = {
    course?: CourseOption;
    star?: number;
    responded?: boolean;
};

export default function RatingsContainer() {
    const [filter, setFilter] = useState<RatingsFilter>({});

    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [ratings, setRatings] = useState<Rating[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);
    const { data: session, status } = useSession();

    const loadMoreRatings = async () => {
        if (status === "authenticated") {
            const result = await getRatingsByInstructor(
                {
                    courseId: filter?.course?.value,
                    star: filter?.star,
                    responded: filter?.responded,
                    limit,
                    skip: skip + limit,
                },
                session.accessToken
            );

            if (!result) return;

            setRatings([...ratings, ...result.items]);
            setSkip(skip + limit);
            setEnd(result.end);
        }
    };

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getRatingsByInstructor(
                    {
                        courseId: filter?.course?.value,
                        star: filter?.star,
                        responded: filter?.responded,
                        limit,
                    },
                    session.accessToken
                );

                if (!result) return;

                setRatings(result.items);
                setSkip(0);
                setEnd(result.end);
                setIsLoading(false);

                // setRatings(ratingsFake);
            })();
        }
    }, [status, filter]);

    return (
        <>
            <div className="mb-5 flex flex-col gap-3">
                <div className="flex gap-3 flex-wrap">
                    <Checkbox
                        disabled={isLoading}
                        checked={filter.responded === true}
                        onChange={(e) =>
                            setFilter({
                                ...filter,
                                responded: e.target.checked || undefined,
                            })
                        }
                        crossOrigin={undefined}
                        label={
                            <Typography
                                color="blue-gray"
                                className="font-medium"
                            >
                                Responded
                            </Typography>
                        }
                    />
                    <Checkbox
                        disabled={isLoading}
                        checked={filter.responded === false}
                        onChange={(e) =>
                            setFilter({
                                ...filter,
                                responded: e.target.checked ? false : undefined,
                            })
                        }
                        crossOrigin={undefined}
                        label={
                            <Typography
                                color="blue-gray"
                                className="font-medium whitespace-nowrap"
                            >
                                Not responded
                            </Typography>
                        }
                    />
                    <div className="w-full md:max-w-[200px]">
                        <Select
                            disabled={isLoading}
                            label="Star"
                            value={filter.star ? `${filter.star}` : ""}
                            onChange={(value) =>
                                setFilter({
                                    ...filter,
                                    star: !!value ? +value : undefined,
                                })
                            }
                        >
                            <Option value="">All</Option>
                            <Option value="1">1 star</Option>
                            <Option value="2">2 star</Option>
                            <Option value="3">3 star</Option>
                            <Option value="4">4 star</Option>
                            <Option value="5">5 star</Option>
                        </Select>
                    </div>
                </div>
                <div>
                    <CourseSelect
                        course={filter?.course}
                        onCourseChange={(c) =>
                            setFilter({
                                ...filter,
                                course: c,
                            })
                        }
                        disabled={isLoading}
                        accessToken={session?.accessToken || ""}
                        instructorId={session?.user.id || ""}
                    />
                </div>
            </div>
            <div>
                {isLoading ? (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                ) : (
                    <RatingList
                        ratings={ratings}
                        end={end}
                        next={loadMoreRatings}
                    />
                )}
            </div>
        </>
    );
}

const loadCourses = async (
    inputValue: string,
    instructorId: string,
    accessToken: string
) => {
    const result = await getInstructorCourses(
        {
            instructorId,
            limit: 50,
            q: inputValue || undefined,
        },
        accessToken
    );

    if (!result) return [];

    return result.items.map((c) => ({
        value: c.id,
        label: c.title,
    }));
};

function CourseSelect({
    course,
    onCourseChange,
    accessToken,
    instructorId,
    disabled,
}: {
    course?: CourseOption;
    onCourseChange: (course?: CourseOption) => void;
    accessToken: string;
    instructorId: string;
    disabled: boolean;
}) {
    return (
        <AsyncSelect
            instanceId="courses"
            placeholder="Select course"
            cacheOptions
            isClearable
            loadOptions={async (value) =>
                await loadCourses(value, instructorId, accessToken)
            }
            isDisabled={disabled}
            value={course}
            onChange={(data) => onCourseChange(data || undefined)}
        />
    );
}
