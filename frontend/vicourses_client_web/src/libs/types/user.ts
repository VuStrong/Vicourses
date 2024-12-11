export type SignInResponse = {
    accessToken: string;
    refreshToken: string;
    user: User;
}

export type RefreshTokenResponse = {
    accessToken: string;
    refreshToken: string;
}

export type UserRole = "admin" | "student" | "instructor";

export type User = {
    id: string;
    name: string;
    email: string;
    emailConfirmed: boolean;
    lockoutEnd: Date | null;
    role: UserRole;
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
    courseTags: string | null;
    categoryIds: string | null;
    paypalAccount?: {
        id: string;
        payerId: string;
        email: string;
    },
}

export type UpdateProfileRequest = {
    name?: string;
    thumbnailToken?: string;
    headline?: string;
    description?: string;
    websiteUrl?: string;
    youtubeUrl?: string;
    facebookUrl?: string;
    linkedInUrl?: string;
    enrolledCoursesVisible?: boolean;
    isPublic?: boolean;
    categoryIds?: string;
}

export type PublicProfile = {
    id: string;
    createdAt: string;
    name: string;
    role: UserRole;
    thumbnailUrl: string | null;
    headline: string | null;
    description: string | null;
    websiteUrl: string | null;
    youtubeUrl: string | null;
    facebookUrl: string | null;
    linkedInUrl: string | null;
    totalEnrollmentCount: number;
}

export type UpdateToInstructorResponse = {
    success: boolean;
    missingRequirements: string[];
}