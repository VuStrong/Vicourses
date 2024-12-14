import { Lesson } from "./lesson";

export type SectionInInstructorCurriculum = {
    id: string;
    courseId: string;
    title: string;
    description: string | null;
    order: number;
    duration: number;
    lessonCount: number;
    lessons: Lesson[];
};

export type Section = {
    id: string;
    courseId: string;
    title: string;
    description: string | null;
    order: number;
}

export type CreateSectionRequest = {
    title: string;
    courseId: string;
    description?: string;
}

export type UpdateSectionRequest = {
    title?: string;
    description?: string;
}