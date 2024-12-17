"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { Session } from "next-auth";
import { Button, Select, Option } from "@material-tailwind/react";

import { Lesson } from "@/libs/types/lesson";
import { CommentSort, LessonComment } from "@/libs/types/lesson-comment";
import { getComments } from "@/services/api/lesson-comment";
import { Loader } from "@/components/common";
import Comment from "./Comment";
import CommentForm from "./CommentForm";

const limit = 10;

export default function CommentsContainer({ lesson }: { lesson: Lesson }) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [comments, setComments] = useState<LessonComment[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [total, setTotal] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);
    const [sort, setSort] = useState<CommentSort>("Newest");
    const { data: session, status } = useSession();

    const getMoreComments = async () => {
        if (status === "authenticated") {
            const result = await getComments(
                lesson.id,
                {
                    skip: skip + limit,
                    limit,
                    sort,
                },
                session.accessToken
            );

            if (result) {
                setComments([...comments, ...result.items]);
                setSkip(skip + limit);
                setEnd(result.end);
                setTotal(result.total);
            }
        }
    };

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getComments(
                    lesson.id,
                    {
                        limit,
                        sort,
                    },
                    session.accessToken
                );

                if (result) {
                    setComments(result.items);
                    setSkip(0);
                    setEnd(result.end);
                    setTotal(result.total);
                    setIsLoading(false);
                }
            })();
        }
    }, [lesson.id, sort, status]);

    return (
        <section className="my-10">
            <div className="w-full mb-10">
                <CommentForm
                    lessonId={lesson.id}
                    session={session}
                    onCommentCreated={(comment) => {
                        setComments([comment, ...comments]);
                        setTotal(total + 1);
                    }}
                />
            </div>

            <h3 className="text-black font-bold text-xl mb-5">
                Comments ({total})
            </h3>

            <div className="max-w-48">
                <Select
                    disabled={isLoading}
                    label="Sort"
                    value={sort}
                    onChange={(value) =>
                        setSort(!!value ? (value as CommentSort) : "Newest")
                    }
                >
                    <Option value="Newest">Newest</Option>
                    <Option value="Oldest">Oldest</Option>
                    <Option value="HighestUpvoted">Highest upvoted</Option>
                </Select>
            </div>

            {isLoading && (
                <div className="flex justify-center">
                    <Loader />
                </div>
            )}

            {!isLoading && (
                <CommentList
                    comments={comments}
                    end={end}
                    next={getMoreComments}
                    session={session}
                />
            )}
        </section>
    );
}

function CommentList({
    comments,
    end,
    next,
    session,
}: {
    comments: LessonComment[];
    end: boolean;
    next: () => Promise<void>;
    session: Session | null;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <>
            {comments.length === 0 ? (
                <div className="flex justify-center text-gray-900">
                    No comment
                </div>
            ) : (
                <div className="flex flex-col mt-10">
                    {comments.map((comment) => (
                        <Comment
                            key={comment.id}
                            initialComment={comment}
                            session={session}
                        />
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
