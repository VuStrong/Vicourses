export type Rating = {
    id: string;
    courseId: string;
    course: CourseInRating | null;
    userId: string;
    user: UserInRating;
    feedback: string;
    star: number;
    createdAt: string;
    responded: boolean;
    response: string | null;
    respondedAt: string | null;
}

type CourseInRating = {
    id: string;
    title: string;
    thumbnailUrl: string | null;
    avgRating: number;
    ratingCount: number;
}

type UserInRating = {
    id: string;
    name: string;
    thumbnailUrl: string | null;
}

export type CreateRatingRequest = {
    courseId: string;
    feedback: string;
    star: number;
}

export type UpdateRatingRequest = {
    feedback?: string;
    star?: number;
}

export type GetRatingsByCourseQuery = {
    courseId: string;
    skip?: number;
    limit?: number;
    star?: number;
    responded?: boolean;
}

export type GetRatingsByInstructorQuery = {
    courseId?: string;
    skip?: number;
    limit?: number;
    star?: number;
    responded?: boolean;
}