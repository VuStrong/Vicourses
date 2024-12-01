export type UserDto = {
    id: string;
    createdAt: Date;
    name: string;
    email: string;
    emailConfirmed: boolean;
    lockoutEnd: Date | null;
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
    paypalAccount: UserPaypalAccountDto | null;
}

export type UserPaypalAccountDto = {
    id: string;
    payerId: string;
    email: string;
}