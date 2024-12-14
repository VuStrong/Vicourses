import { VideoStatus } from "./common";

export type LessonType = "Video" | "Quiz";

export type Quiz = {
    number: number;
    title: string;
    isMultiChoice: boolean;
    answers: {
        number: number;
        title: string;
        isCorrect: boolean;
        explanation: string | null;
    }[];
};

export type Lesson = {
    id: string;
    courseId: string;
    sectionId: string;
    title: string;
    order: number;
    type: LessonType;
    createdAt: string;
    updatedAt: string;
    description: string | null;
    video: {
        fileId: string;
        url: string;
        originalFileName: string;
        streamFileUrl: string | null;
        duration: number;
        status: VideoStatus;
    } | null;
    quizzesCount: number;
    quizzes: Quiz[];
};

export type CreateLessonRequest = {
    title: string;
    courseId: string;
    sectionId: string;
    type: LessonType;
    description?: string;
}

export type UpdateLessonRequest = {
    title?: string;
    description?: string;
    videoToken?: string;
}

export type AddOrUpdateQuizRequest = {
    title: string;
    answers: {
        title: string;
        isCorrect: boolean;
        explanation?: string;
    }[];
}