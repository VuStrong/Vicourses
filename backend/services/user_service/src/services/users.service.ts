import { EntityPropertyNotFoundError, QueryFailedError } from "typeorm";
import crypto from "crypto";
import bcrypt from "bcrypt";
import { AppError } from "../utils/app-error";
import logger from "../logger";
import Config from "../config";
import * as rabbitmq from "../rabbitmq/publisher";
import User, { Role } from "../entities/user.entity";
import { usersRepository } from "../data/repositories";
import * as jwt from "../utils/jwt";
import { GetUsersPayload, UpdateProfilePayload } from "./payloads";
import PagedResult from "../utils/paged-result";
import { UserDto } from "../dtos/user-dtos";
import { mapUserEntityToUserDto } from "../dtos/mapper";

/**
 * Get one user by Id
 * @param userId - ID of user
 * @param fields - An array contains fields to select (undefined to select all)
 * @returns User
 */
export async function getUserById(userId: string, fields?: (keyof User)[]): Promise<UserDto> {
    if (fields) fields.push("id");

    try {
        const user = await usersRepository.findOne({
            where: { id: userId },
            select: fields,
        });
    
        if (!user) {
            throw new AppError("User not found", 404);
        }
    
        return mapUserEntityToUserDto(user);
    } catch (error) {
        if (error instanceof EntityPropertyNotFoundError) {
            throw new AppError(error.message, 400);
        }

        throw error;
    }
}

export async function getUsers(payload: GetUsersPayload): Promise<PagedResult<UserDto>> {
    const skip = (payload.skip && payload.skip >= 0) ? payload.skip : 0;
    const limit = (payload.limit && payload.limit > 0) ? payload.limit : 15;

    if (payload.fields) payload.fields.push("id");

    try {
        const [users, total] = await usersRepository.findAndCount({
            where: {
                role: payload.role,
            },
            order: payload.order,
            skip,
            take: limit,
            select: payload.fields,
        });
    
        return new PagedResult<UserDto>(
            users.map(u => mapUserEntityToUserDto(u)), 
            skip, 
            limit, 
            total,
        );
    } catch (error) {
        if (error instanceof EntityPropertyNotFoundError) {
            throw new AppError(error.message, 400);
        }

        throw error;
    }
}

/**
 * Create an user with password
 */
export async function createUser({
    name,
    email,
    password
}: {
    name: string,
    email: string,
    password: string
}): Promise<UserDto> {
    const hashedPassword = await bcrypt.hash(password, 10);

    const user = usersRepository.create({
        name,
        email,
        hashedPassword,
    });
    
    try {
        await usersRepository.insert(user);

        await sendEmailConfirmationLink(email);

        const userDto = mapUserEntityToUserDto(user);
        await rabbitmq.publishUserCreatedEvent(userDto);

        logger.info(`New user registered`, {
            id: user.id,
            email: user.email,
        });

        return userDto;
    } catch (error) {
        if (error instanceof QueryFailedError) {
            if (error.driverError.code === "ER_DUP_ENTRY") {
                throw new AppError(`Email ${email} already exists`, 400);
            }
        }

        throw error;
    }
}

/**
 * Lock the user
 * @param userId - ID of user to lock
 * @param days - Number of days to lock user. If value is 0 or negative, user will be unlocked. Default is 30 (days)
 */
export async function setLockedOut(userId: string, days: number = 30) {
    const user = await usersRepository.findOne({
        where: { id: userId },
        select: {
            role: true,
            lockoutEnd: true,
        },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }
    if (user.role === Role.ADMIN) {
        throw new AppError("Action on this user is not allowed", 403);
    }

    if (days <= 0) {
        await usersRepository.update(userId, { lockoutEnd: null });

        logger.info(`User ${userId} unlocked`);
    } else {
        const lockTo = new Date();
        lockTo.setDate(lockTo.getDate() + days);

        await usersRepository.update(userId, { lockoutEnd: lockTo });

        logger.info(`User ${userId} is locked until ${lockTo}`);
    }
}

export async function setRole(userId: string, role: Role) {
    const user = await usersRepository.findOne({
        where: { id: userId },
        select: {
            role: true,
            lockoutEnd: true,
        },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }
    if (user.role === Role.ADMIN) {
        throw new AppError("Action on this user is not allowed", 403);
    }
    if (user.role === role) return;

    user.role = role;

    await usersRepository.save(user);

    await rabbitmq.publishUserRoleUpdatedEvent({
        id: userId,
        role,
    });
}

export async function changePassword(userId: string, oldPassword: string, newPassword: string) {
    const user = await usersRepository.findOne({
        where: { id: userId },
        select: {
            id: true,
            hashedPassword: true,
        },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }
    if (user.hashedPassword === null) {
        throw new AppError("Your account is not registered with email and password. Try reset your password", 403);
    }

    if (!(await bcrypt.compare(oldPassword, user.hashedPassword))) {
        throw new AppError("Password does not match", 403);
    }

    const newHashedPassword = await bcrypt.hash(newPassword, 10);

    await usersRepository.update(userId, { hashedPassword: newHashedPassword });
}

export async function confirmEmail(userId: string, token: string) {
    const user = await usersRepository.findOne({
        where: { id: userId },
        select: {
            emailConfirmed: true,
            emailConfirmationToken: true,
            emailConfirmationTokenExpiryTime: true,
        },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (user.emailConfirmed) {
        return;
    }

    if (user.emailConfirmationToken &&
        user.emailConfirmationTokenExpiryTime &&
        token === user.emailConfirmationToken &&
        user.emailConfirmationTokenExpiryTime > new Date()
    ) {
        await usersRepository.update(userId, {
            emailConfirmed: true,
            emailConfirmationToken: null,
            emailConfirmationTokenExpiryTime: null,
        });
    } else {
        throw new AppError("Invalid token", 403);
    }
}

/**
 * Generate an email confirmation link and send it to user email
 */
export async function sendEmailConfirmationLink(email: string) {
    const user = await usersRepository.findOne({
        where: { email },
        select: {
            id: true,
            name: true,
            email: true,
            emailConfirmed: true,
        }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (!user.emailConfirmed) {
        const emailConfirmationToken = crypto.randomBytes(64).toString('hex');
        const date = new Date();
        date.setDate(date.getDate() + 1);

        const webUrl = Config.WebUrls.ConfirmEmail;
        const link = `${webUrl}?token=${emailConfirmationToken}&userId=${user.id}`;

        await usersRepository.update(user.id, {
            emailConfirmationToken,
            emailConfirmationTokenExpiryTime: date,
        });

        await rabbitmq.sendToEmailQueue({
            to: user.email,
            template: "confirm_email",
            payload: {
                username: user.name,
                link,
            }
        });
    }
}

export async function resetPassword(userId: string, token: string, newPassword: string) {
    const user = await usersRepository.findOne({
        where: { id: userId },
        select: {
            id: true,
            passwordResetToken: true,
            passwordResetTokenExpiryTime: true,
        }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (user.passwordResetToken &&
        user.passwordResetTokenExpiryTime &&
        token === user.passwordResetToken &&
        user.passwordResetTokenExpiryTime > new Date()
    ) {
        const hashedPassword = await bcrypt.hash(newPassword, 10);
        await usersRepository.update(user.id, {
            hashedPassword,
            passwordResetToken: null,
            passwordResetTokenExpiryTime: null,
        });
    } else {
        throw new AppError("Invalid token", 403);
    }
}

/**
 * Generate a password reset link and send it to user email
 */
export async function sendPasswordResetLink(email: string) {
    const user = await usersRepository.findOne({
        where: { email },
        select: {
            id: true,
            name: true,
            email: true,
        },
    });

    if (!user) {
        throw new AppError("Email not found", 404);
    }

    const token = crypto.randomBytes(64).toString('hex');
    const date = new Date();
    date.setDate(date.getDate() + 1);

    const webUrl = Config.WebUrls.ResetPassword;
    const link = `${webUrl}?token=${token}&userId=${user.id}`;

    await usersRepository.update(user.id, {
        passwordResetToken: token,
        passwordResetTokenExpiryTime: date,
    });

    await rabbitmq.sendToEmailQueue({
        to: user.email,
        template: "reset_password",
        payload: {
            username: user.name,
            link,
        }
    });
}

export async function updateUserProfile(id: string, payload: UpdateProfilePayload): Promise<UserDto> {
    let user = await usersRepository.findOne({
        where: { id }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (payload.name) user.name = payload.name;
    if (payload.headline) user.headline = payload.headline;
    if (payload.description) user.description = payload.description;
    if (payload.websiteUrl) user.websiteUrl = payload.websiteUrl;
    if (payload.youtubeUrl) user.youtubeUrl = payload.youtubeUrl;
    if (payload.facebookUrl) user.facebookUrl = payload.facebookUrl;
    if (payload.linkedInUrl) user.linkedInUrl = payload.linkedInUrl;
    if (payload.categoryIds) user.categoryIds = payload.categoryIds;
    if (payload.enrolledCoursesVisible !== undefined) user.enrolledCoursesVisible = payload.enrolledCoursesVisible;
    if (payload.isPublic !== undefined) user.isPublic = payload.isPublic;

    if (payload.thumbnailToken) {
        try {
            const { fileId, url, userId } = jwt.verifyFileUploadToken(payload.thumbnailToken);

            if (user.id !== userId) {
                throw new Error();
            }

            user.thumbnailId = fileId;
            user.thumbnailUrl = url;
        } catch {
            throw new AppError("Invalid thumbnailToken", 403);
        }
    }
    
    await usersRepository.save(user);

    const userDto = mapUserEntityToUserDto(user);

    await rabbitmq.publishUserInfoUpdatedEvent(userDto);

    return userDto;
}