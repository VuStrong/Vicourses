import User from "../entities/user.entity";
import { UserDto } from "./user-dtos";

export function mapUserEntityToUserDto(user: User): UserDto {
    let paypalAccount: any = undefined;

    if (user.paypalAccount !== undefined) {
        paypalAccount = user.paypalAccount ? {
            id: user.paypalAccount.id,
            payerId: user.paypalAccount.payerId,
            email: user.paypalAccount.email,
        } : null;
    }

    return {
        id: user.id,
        createdAt: user.createdAt,
        name: user.name,
        email: user.email,
        emailConfirmed: user.emailConfirmed,
        lockoutEnd: user.lockoutEnd,
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
        paypalAccount: paypalAccount,
    };
}
