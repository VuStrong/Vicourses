import { QueryFailedError } from "typeorm";
import * as bcrypt from "bcrypt";
import { userRepository } from "../data/user.repository";
import User, { Role } from "../entity/user.entity";
import { AppError } from "../utils/app-error";
import logger from "../logger";
import { EventType, event } from "../events";
import { tokenRepository } from "../data/token.repository";
import { TokenType } from "../entity/token.entity";
import Config from "../config";
import * as rabbitmq from "../rabbitmq/client";

export async function getUserById(userId: string): Promise<User> {
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    return user;
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
}): Promise<User> {
    const passwordHash = await bcrypt.hash(password, 10);

    const user = userRepository.create({
        name,
        email,
        passwordHash
    });
    
    try {
        const newUser = await userRepository.save(user);

        event.emit(EventType.USER_CREATED, newUser);

        logger.info(`New user registered`, {
            id: newUser.id,
            email: newUser.email,
        });

        return newUser;
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
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }
    if (user.role === Role.ADMIN) {
        throw new AppError("Action on this user is not allowed", 403);
    }

    if (days <= 0) {
        await userRepository.update(userId, { lockoutEnd: null });

        logger.info(`User ${userId} unlocked`);
    } else {
        const lockTo = new Date();
        lockTo.setDate(lockTo.getDate() + days);

        await userRepository.update(userId, { lockoutEnd: lockTo });

        logger.info(`User ${userId} is locked until ${lockTo}`);
    }
}

export async function setRole(userId: string, role: Role) {
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }
    if (user.role === Role.ADMIN) {
        throw new AppError("Action on this user is not allowed", 403);
    }
    if (user.role === role) return;

    user.role = role;

    await userRepository.save(user);

    event.emit(EventType.USER_ROLE_UPDATED, user);
}

export async function changePassword(userId: string, oldPassword: string, newPassword: string) {
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (!(await bcrypt.compare(oldPassword, user.passwordHash))) {
        throw new AppError("Password does not match", 403);
    }

    const newHashedPassword = await bcrypt.hash(newPassword, 10);

    await userRepository.update(userId, { passwordHash: newHashedPassword });
}

export async function confirmEmail(userId: string, token: string) {
    const user = await userRepository.findOne({
        where: { id: userId },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (user.emailConfirmed) {
        return;
    }

    const tokenInDb = await tokenRepository.findOne({
        where: {
            token,
            userId,
        },
    });

    if (
        !tokenInDb ||
        tokenInDb.type !== TokenType.CONFIRM_EMAIL ||
        tokenInDb.expiryTime < new Date()
    ) {
        throw new AppError("Invalid token", 401);
    }

    await userRepository.update(user.id, { emailConfirmed: true });

    await tokenRepository.delete({
        token,
        userId,
    });
}

/**
 * Generate an email confirmation link and send it to user email
 */
export async function sendEmailConfirmationLink(email: string) {
    const user = await userRepository.findOne({
        where: { email },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (!user.emailConfirmed) {
        const confirmEmailToken =
            await tokenRepository.generateConfirmEmailToken(user.id);

        const webUrl = Config.WEB_CLIENT_EMAIL_CONFIRM_URL;
        const link = `${webUrl}?token=${confirmEmailToken}&userId=${user.id}`;

        rabbitmq.sendToQueue(rabbitmq.Queue.SEND_EMAIL, {
            to: user.email,
            emailType: "confirm_email",
            payload: {
                userName: user.name,
                link,
            }
        });
    }
}

export async function resetPassword(userId: string, token: string, newPassword: string) {
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    const tokenInDb = await tokenRepository.findOne({
        where: {
            token, userId
        }
    });

    if (
        !tokenInDb ||
        tokenInDb.type !== TokenType.RESET_PASSWORD ||
        tokenInDb.expiryTime < new Date()
    ) {
        throw new AppError("Invalid token", 401);
    }

    const passwordHash = await bcrypt.hash(newPassword, 10);
    await userRepository.update(user.id, { passwordHash });

    await tokenRepository.delete({
        token,
        userId
    });
}

/**
 * Generate a password reset link and send it to user email
 */
export async function sendPasswordResetLink(email: string) {
    const user = await userRepository.findOne({
        where: { email },
    });

    if (!user) {
        throw new AppError("Email not found", 404);
    }

    const token = await tokenRepository.generateResetPasswordToken(user.id);
    const webUrl = Config.WEB_CLIENT_RESET_PASSWORD_URL;
    const link = `${webUrl}?token=${token}&userId=${user.id}`;

    rabbitmq.sendToQueue(rabbitmq.Queue.SEND_EMAIL, {
        to: user.email,
        emailType: "reset_password",
        payload: {
            userName: user.name,
            link,
        }
    });
}
