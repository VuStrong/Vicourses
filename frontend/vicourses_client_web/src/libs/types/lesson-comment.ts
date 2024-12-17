export type LessonComment = {
    id: string;
    lessonId: string;
    instructorId: string;
    user: {
        id: string;
        name: string;
        thumbnailUrl: string | null;
    };
    content: string;
    createdAt: string;
    isDeleted: boolean;
    replyToId: string | null;
    replyCount: number;
    upvoteCount: number;
    userUpvoteIds: string[];
};

export type CreateLessonCommentRequest = {
    content: string;
    replyToId?: string;
}

export type CommentSort = "Newest" | "Oldest" | "HighestUpvoted";

export type GetLessonCommentsQuery = {
    replyToId?: string;
    sort?: CommentSort;
    skip?: number;
    limit?: number;
}