import { usersRepository } from "../data/repositories";
import User, { Role } from "../entities/user.entity";
import { AppError } from "../utils/app-error";
import * as rabbitmq from "../rabbitmq/publisher";

type InstructorCheckResult = {
    isValid: boolean;
    missingRequiments: string[];
}

/**
 * check if user is qualified to become an instructor?
 * @param userId - ID of user
 */
export async function checkInstructorInfo(user: User): Promise<InstructorCheckResult> {
    const result: InstructorCheckResult = {
        isValid: true,
        missingRequiments: [],
    };
    
    if (!user.emailConfirmed) {
        result.isValid = false;
        result.missingRequiments.push("Your email must be verified.");
    }
    if (!user.thumbnailUrl) {
        result.isValid = false;
        result.missingRequiments.push("You must upload a thumbnail for your account.");
    }
    if (!user.headline) {
        result.isValid = false;
        result.missingRequiments.push("You must set your profile headline.");
    }
    if (!user.description || user.description.length < 100) {
        result.isValid = false;
        result.missingRequiments.push("You must write description for your profile, contains more than 100 characters.");
    }
    if (!user.paypalAccount) {
        result.isValid = false;
        result.missingRequiments.push("You must link your paypal account.");
    }
    
    return result;
}

/**
 * Update an user role to 'instructor'
 * @param userId - ID of user
 */
export async function upgradeToInstructor(userId: string) {
    const user = await usersRepository.findOne({
        where: { id: userId },
        relations: {
            paypalAccount: true,
        }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }
    if (user.role !== Role.STUDENT) {
        if (user.role === Role.INSTRUCTOR) return;

        throw new AppError("Action on this user is not allowed", 403);
    }

    const result = await checkInstructorInfo(user);

    if (!result.isValid) {
        throw new AppError("Your profile is missing some information", 403, result.missingRequiments);
    }

    await usersRepository.update(user.id, {
        role: Role.INSTRUCTOR,
    });

    await rabbitmq.publishUserRoleUpdatedEvent({
        id: userId,
        role: Role.INSTRUCTOR,
    });
}