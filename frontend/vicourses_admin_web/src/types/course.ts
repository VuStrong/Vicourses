import { VideoFile } from "./common";

export type CourseLevel = "All" | "Basic" | "Intermediate" | "Expert";
export type CourseStatus = "Unpublished" | "WaitingToVerify" | "Published";
export type LessonType = "Video" | "Quiz";

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

export type InstructorCurriculum = {
    sections: SectionInInstructorCurriculum[];
}

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
    userId: string;
    title: string;
    order: number;
    type: LessonType;
    createdAt: string;
    updatedAt: string;
    description: string | null;
    video: VideoFile | null;
    quizzesCount: number;
    quizzes: Quiz[];
};