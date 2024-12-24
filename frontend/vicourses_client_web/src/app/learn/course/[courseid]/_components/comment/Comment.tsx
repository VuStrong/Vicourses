"use client";

import { useMemo, useState } from "react";
import { Session } from "next-auth";
import {
    Avatar,
    Button,
    Menu,
    MenuHandler,
    MenuItem,
    MenuList,
} from "@material-tailwind/react";
import { FaArrowCircleUp, FaReply } from "react-icons/fa";
import { HiDotsHorizontal } from "react-icons/hi";
import toast from "react-hot-toast";

import { LessonComment } from "@/libs/types/lesson-comment";
import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import {
    cancelUpvoteComment,
    deleteLessonComment,
    getComments,
    upvoteComment,
} from "@/services/api/lesson-comment";
import CommentForm from "./CommentForm";

export default function Comment({
    initialComment,
    session,
}: {
    initialComment: LessonComment;
    session: Session | null;
}) {
    const [comment, setComment] = useState<LessonComment>(initialComment);
    const [disabled, setDisabled] = useState<boolean>(false);
    const [openReplyForm, setOpenReplyForm] = useState<boolean>(false);
    const [isLoadingReplies, setIsLoadingReplies] = useState<boolean>(false);
    const [replies, setReplies] = useState<LessonComment[]>([]);

    const allowToDelete =
        session?.user.id === comment.instructorId ||
        session?.user.id === comment.user.id;

    const isUpvoted = useMemo(() => {
        if (!session) return false;

        return comment.userUpvoteIds.includes(session.user.id);
    }, [comment, session?.user.id]);

    const handleClickUpvote = async () => {
        if (!session || disabled) return;

        setDisabled(true);

        try {
            if (isUpvoted) {
                await cancelUpvoteComment(
                    comment.lessonId,
                    comment.id,
                    session.accessToken
                );

                setComment({
                    ...comment,
                    upvoteCount: comment.upvoteCount - 1,
                    userUpvoteIds: comment.userUpvoteIds.filter(
                        (u) => u !== session.user.id
                    ),
                });
            } else {
                await upvoteComment(
                    comment.lessonId,
                    comment.id,
                    session.accessToken
                );

                setComment({
                    ...comment,
                    upvoteCount: comment.upvoteCount + 1,
                    userUpvoteIds: [...comment.userUpvoteIds, session.user.id],
                });
            }
        } catch (error) {}

        setDisabled(false);
    };

    const handleDeleteComment = async () => {
        if (!session || disabled) return;

        setDisabled(true);

        try {
            await deleteLessonComment(
                comment.lessonId,
                comment.id,
                session?.accessToken || ""
            );

            setComment({
                ...comment,
                isDeleted: true,
            });
        } catch (error: any) {
            toast.error(error.message);
        }

        setDisabled(false);
    };

    const getReplies = async () => {
        if (!session || isLoadingReplies) return;

        setIsLoadingReplies(true);

        const result = await getComments(
            comment.lessonId,
            {
                skip: replies.length,
                limit: 5,
                replyToId: comment.id,
            },
            session.accessToken
        );

        if (result) {
            setReplies([...replies, ...result.items]);
            setIsLoadingReplies(false);
        }
    };

    return (
        <div className="border-b border-gray-400 py-5">
            <div className="flex gap-2 items-center">
                <Avatar
                    src={comment.user.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                    alt={comment.user.name}
                />
                <div className="text-black font-semibold line-clamp-1">
                    {comment.user.name}
                </div>
                <div className="text-xs text-gray-800">
                    {new Date(comment.createdAt).toLocaleDateString()}
                </div>
            </div>

            {comment.isDeleted ? (
                <div className="my-3 text-gray-700 font-semibold">
                    <em>Comment deleted</em>
                </div>
            ) : (
                <div
                    className="my-3 text-gray-700"
                    dangerouslySetInnerHTML={{
                        __html: comment.content,
                    }}
                />
            )}

            {!comment.isDeleted && (
                <div className="flex gap-5 items-center">
                    <div className="flex gap-1 items-center">
                        <button
                            title={isUpvoted ? "Cancel upvote" : "Upvote"}
                            onClick={handleClickUpvote}
                            className={`${isUpvoted && "text-primary"}`}
                        >
                            <FaArrowCircleUp size={20} />
                        </button>
                        {comment.upvoteCount}
                    </div>

                    {!comment.replyToId && (
                        <div className="flex gap-1 items-center">
                            <button
                                title="Reply"
                                onClick={() => setOpenReplyForm(!openReplyForm)}
                                className={`${openReplyForm && "text-primary"}`}
                            >
                                <FaReply size={20} />
                            </button>
                            {comment.replyCount}
                        </div>
                    )}

                    {allowToDelete && (
                        <Menu>
                            <MenuHandler>
                                <button>
                                    <HiDotsHorizontal size={20} />
                                </button>
                            </MenuHandler>
                            <MenuList>
                                <MenuItem onClick={handleDeleteComment}>
                                    Delete
                                </MenuItem>
                            </MenuList>
                        </Menu>
                    )}
                </div>
            )}

            {openReplyForm && (
                <div className="my-5">
                    <CommentForm
                        lessonId={comment.lessonId}
                        replyTo={comment}
                        session={session}
                        onCommentCreated={(createdComment) => {
                            setReplies([...replies, createdComment]);
                            setComment({
                                ...comment,
                                replyCount: comment.replyCount + 1,
                            });
                            setOpenReplyForm(false);
                        }}
                    />
                </div>
            )}

            {replies.length > 0 && (
                <div className="pl-10">
                    {replies.map((reply) => (
                        <Comment
                            key={reply.id}
                            initialComment={reply}
                            session={session}
                        />
                    ))}
                </div>
            )}

            {comment.replyCount > 0 && replies.length < comment.replyCount && (
                <div className="mt-5">
                    <Button
                        variant="text"
                        size="sm"
                        loading={isLoadingReplies}
                        onClick={getReplies}
                    >
                        Load replies
                    </Button>
                </div>
            )}
        </div>
    );
}
