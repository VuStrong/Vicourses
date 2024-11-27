import User from "../entities/user.entity";
import { UserDto } from "./user-dtos";

export function mapUserEntityToUserDto(user: User): UserDto {
    return {
        id: user.id,
        createdAt: user.createdAt,
        name: user.name,
        email: user.email,
        emailConfirmed: user.emailConfirmed,
        lockoutEnd: user.lockoutEnd,
        isLocked: user.isLocked(),
        role: user.role,
        thumbnailUrl: user.thumbnailUrl,
        headline: user.headline,
        description: user.description,
        websiteUrl: user.websiteUrl,
        youtubeUrl: user.youtubeUrl,
        facebookUrl: user.facebookUrl,
        linkedInUrl: user.linkedInUrl,
        enrolledCoursesVisible: user.enrolledCoursesVisible,
        isPublic: user.isPublic,
        totalEnrollmentCount: user.totalEnrollmentCount,
        courseTags: user.courseTags,
        categoryIds: user.categoryIds,
    };
}
