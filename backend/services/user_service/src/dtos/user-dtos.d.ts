export type UserDto = {
    id: string;
    createdAt: Date;
    name: string;
    email: string;
    emailConfirmed: boolean;
    lockoutEnd: Date | null;
    isLocked: boolean;
    role: string;
    thumbnailUrl: string | null;
    headline: string | null;
    description: string | null;
    websiteUrl: string | null;
    youtubeUrl: string | null;
    facebookUrl: string | null;
    linkedInUrl: string | null;
    enrolledCoursesVisible: boolean;
    isPublic: boolean;
    totalEnrollmentCount: number;
    courseTags: string;
    categoryIds: string;
}
