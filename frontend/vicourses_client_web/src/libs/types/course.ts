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
    previewVideo: {
        fileId: string;
        url: string;
        originalFileName: string;
        streamFileUrl: string | null;
        duration: number;
        status: string;
    },
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

export type SearchCoursesQuery = {
    sort?: "Relevance" | "Newest" | "HighestRated";
    q?: string;
    free?: boolean;
    level?: CourseLevel;
    rating?: number;
    skip?: number;
    limit?: number;
}

export type CourseLevel = "All" | "Basic" | "Intermediate" | "Expert";
export type CourseStatus = "Unpublished" | "WaitingToVerify" | "Published";