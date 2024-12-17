import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import {
    CreateLessonCommentRequest,
    GetLessonCommentsQuery,
    LessonComment,
} from "@/libs/types/lesson-comment";

export async function getComments(
    lessonId: string,
    query: GetLessonCommentsQuery,
    accessToken: string
): Promise<PagedResult<LessonComment> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/comments?${params}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function createLessonComment(
    lessonId: string,
    request: CreateLessonCommentRequest,
    accessToken: string
): Promise<LessonComment> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/comments`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
            body: JSON.stringify(request),
        }
    );

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function deleteLessonComment(
    lessonId: string,
    commentId: string,
    accessToken: string
) {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/comments/${commentId}`,
        {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function upvoteComment(
    lessonId: string,
    commentId: string,
    accessToken: string
) {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/comments/${commentId}/upvote`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function cancelUpvoteComment(
    lessonId: string,
    commentId: string,
    accessToken: string
) {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/comments/${commentId}/cancel-upvote`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}