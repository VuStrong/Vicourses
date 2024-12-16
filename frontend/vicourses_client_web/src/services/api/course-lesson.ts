import { BACKEND_URL } from "@/libs/constants";
import {
    AddOrUpdateQuizRequest,
    CreateLessonRequest,
    Lesson,
    UpdateLessonRequest,
} from "@/libs/types/lesson";

export async function getLesson(lessonId: string, accessToken: string): Promise<Lesson | null> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/lessons/${lessonId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    return await res.json();
}

export async function createLesson(
    request: CreateLessonRequest,
    accessToken: string
): Promise<Lesson> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/lessons`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function updateLesson(
    lessonId: string,
    request: UpdateLessonRequest,
    accessToken: string
): Promise<Lesson> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/lessons/${lessonId}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function deleteLesson(lessonId: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/lessons/${lessonId}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function addQuizToLesson(
    lessonId: string,
    request: AddOrUpdateQuizRequest,
    accessToken: string
): Promise<Lesson> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/quizzes`,
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

export async function updateQuizInLesson(
    lessonId: string,
    quizNumber: number,
    request: AddOrUpdateQuizRequest,
    accessToken: string
): Promise<Lesson> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/quizzes/${quizNumber}`,
        {
            method: "PATCH",
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

export async function removeQuizFromLesson(
    lessonId: string,
    quizNumber: number,
    accessToken: string
): Promise<Lesson> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/lessons/${lessonId}/quizzes/${quizNumber}`,
        {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}