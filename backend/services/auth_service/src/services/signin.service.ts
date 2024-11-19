import { OAuth2Client, TokenPayload } from "google-auth-library";
import * as bcrypt from "bcrypt";
import { userRepository } from "../data/user.repository";
import User from "../entity/user.entity";
import { AppError } from "../utils/app-error";
import * as jwt from "../utils/jwt";
import { tokenRepository } from "../data/token.repository";
import logger from "../logger";
import Config from "../config";
import { EventType, event } from "../events";
import { TokenType } from "../entity/token.entity";

const client = new OAuth2Client(Config.GOOGLE_CLIENT_ID);

export async function signInWithEmailAndPassword(email: string, password: string): Promise<{
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

    if (user.isLocked()) {
        throw new AppError("You are locked out!", 403);
    }

    const accessToken = await jwt.signForUser(user);
    const refreshToken = await tokenRepository.generateRefreshToken(user.id);

    logger.info("User logged in", {
        email: user.email
    });

    return {
        user,
        accessToken,
        refreshToken
    }
}

export async function signInWithGoogle(idToken: string): Promise<{
    accessToken: string;
    refreshToken: string;
    user: User;
}> {
    let payload: TokenPayload | undefined;

    try {
        const ticket = await client.verifyIdToken({
            idToken: idToken,
            audience: Config.GOOGLE_CLIENT_ID,
        });
        payload = ticket.getPayload();
    } catch (error) {
        throw new AppError("Invalid credentials", 401);
    }

    if (!payload?.email) {
        throw new AppError("Invalid credentials", 401);
    }

    let user = await userRepository.findOne({
        where: { email: payload.email },
    });

    let accessToken = "";
    let refreshToken = "";

    if (user) {
        if (user.isLocked()) {
            throw new AppError("You are locked out!", 403);
        }

        if (!user.emailConfirmed) {
            await userRepository.update(user.id, { emailConfirmed: true });
            user.emailConfirmed = true;
        }

        accessToken = await jwt.signForUser(user);
        refreshToken = await tokenRepository.generateRefreshToken(user.id);

        logger.info("User logged in with google account", {
            email: user.email
        });
    } else {
        user = userRepository.create({
            name: payload.name,
            email: payload.email,
            emailConfirmed: true,
        });

        user = await userRepository.save(user);

        event.emit(EventType.USER_CREATED, user);

        logger.info(`New user registered with google account`, {
            id: user.id,
            email: user.email,
        });

        accessToken = await jwt.signForUser(user);
        refreshToken = await tokenRepository.generateRefreshToken(user.id);
    }

    return {
        accessToken,
        refreshToken,
        user,
    };
}

export async function refreshAccessToken(userId: string, refreshToken: string): Promise<string> {
    const user = await userRepository.findOne({
        where: { id: userId }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (user.isLocked()) {
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