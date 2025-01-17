import { VideoFile } from "./common";
import { LessonType } from "./lesson";
import { SectionInInstructorCurriculum } from "./section";

export type CourseLevel = "All" | "Basic" | "Intermediate" | "Expert";
export type CourseStatus = "Unpublished" | "WaitingToVerify" | "Published";

export type Course = {
    id: string;
    title: string;
    titleCleaned: string;
    learnedContents: string[];
    level: CourseLevel;
    status: CourseStatus;
    isApproved: boolean;
    isPaid: boolean;
    price: number;
    rating: number;
    createdAt: string;
    updatedAt: string;
    studentCount: number;
    locale: {
        name: string;
        englishTitle: string;
    } | null,
    thumbnailUrl: string | null;
    user: {
        id: string;
        name: string;
        thumbnailUrl: string | null;
    },
    category: {
        id: string;
        name: string;
        slug: string;
    },
    subCategory: {
        id: string;
        name: string;
        slug: string;
    },
    tags: string[],
}

export type CourseDetail = Course & {
    description: string | null;
    requirements: string[],
    targetStudents: string[],
    previewVideo: VideoFile | null,
    metrics: {
        sectionsCount: number;
        lessonsCount: number;
        quizLessonsCount: number;
        totalVideoDuration: number;
    },
}

export type GetCoursesQuery = {
    sort?: "Newest" | "HighestRated" | "PriceDesc" | "PriceAsc";
    q?: string;
    categoryId?: string;
    subCategoryId?: string;
    free?: boolean;
    level?: CourseLevel;
    rating?: number;
    status?: CourseStatus;
    tag?: string;
    skip?: number;
    limit?: number;
}

export type GetInstructorCoursesQuery = {
    instructorId: string;
    q?: string;
    status?: CourseStatus;
    skip?: number;
    limit?: number;
}

export type SearchCoursesQuery = {
    sort?: "Relevance" | "Newest" | "HighestRated";
    q?: string;
    free?: boolean;
    level?: CourseLevel;
    rating?: number;
    skip?: number;
    limit?: number;
}

export type PublicCurriculum = {
    totalDuration: number;
    totalSection: number;
    totalLesson: number;
    sections: {
        id: string;
        courseId: string;
        title: string;
        order: number;
        duration: number;
        lessonCount: number;
        lessons: {
            id: string;
            title: string;
            order: number;
            type: LessonType;
            duration: number;
            quizzesCount: number;
        }[];
    }[];
}

export type InstructorCurriculum = {
    sections: SectionInInstructorCurriculum[];
}

export type CreateCourseRequest = {
    title: string;
    categoryId: string;
    subCategoryId: string;
}

export type UpdateCourseRequest = {
    title?: string;
    categoryId?: string;
    subCategoryId?: string;
    description?: string;
    tags?: string[];
    requirements?: string[];
    targetStudents?: string[];
    learnedContents?: string[];
    price?: number;
    locale?: string;
    level?: CourseLevel;
    status?: CourseStatus;
    thumbnailToken?: string;
    previewVideoToken?: string;
}

export type CourseCheckResponse = {
    isValid: boolean;
    missingRequirements: string[];
}

export type UpdateCurriculumRequest = {
    items: {
        id: string;
        type: "Section" | "Lesson";
    }[];
}