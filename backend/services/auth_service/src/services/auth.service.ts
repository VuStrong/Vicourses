import { QueryFailedError } from "typeorm";
import * as bcrypt from "bcrypt";
import * as jwt from "../utils/jwt";
import { userRepository } from "../data/user.repository";
import { tokenRepository } from "../data/token.repository";
import User from "../entity/user.entity";
import { TokenType } from "../entity/token.entity";
import { EventType, event } from "../events";
import { AppError } from "../utils/app-error";

export async function login(email: string, password: string): Promise<{
    accessToken: string,
    refreshToken: string,
    user: User,
}> {
    const user = await userRepository.findOne({
        where: { email },
    });

    if (!user) {
        throw new AppError("Invalid credentials", 401);
    }

    const isMatchPassword = await bcrypt.compare(password, user.passwordHash);

    if (!isMatchPassword) {
        throw new AppError("Invalid credentials", 401);
    }

    if (user.locked) {
        throw new AppError("You are locked out!", 403);
    }

    const accessToken = await jwt.signForUser(user);
    const refreshToken = await tokenRepository.generateRefreshToken(user.id);

    return {
        user,
        accessToken,
        refreshToken
    }
}

export async function register({
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

export async function refreshToken(userId: string, refreshToken: string): Promise<string> {
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (user.locked) {
        throw new AppError("You are locked out!", 403);
    }

    const token = await tokenRepository.findOne({
        where: {
            token: refreshToken,
            userId
        }
    });

    if (!token || token.type !== TokenType.REFRESH_TOKEN || token.expiryTime < new Date()) {
        throw new AppError("Invalid token", 401);
    }

    const accessToken = await jwt.signForUser(user);

    return accessToken;
}

export async function revokeRefreshToken(userId: string, refreshToken: string) {
    await tokenRepository.delete({
        token: refreshToken,
        userId
    });
}