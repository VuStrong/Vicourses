import { OAuth2Client, TokenPayload } from "google-auth-library";
import bcrypt from "bcrypt";
import { AppError } from "../utils/app-error";
import logger from "../logger";
import Config from "../config";
import { refreshTokensRepository, usersRepository } from "../data/repositories";
import * as jwt from "../utils/jwt";
import * as rabbitmq from "../rabbitmq/publisher";
import { UserDto } from "../dtos/user-dtos";
import { mapUserEntityToUserDto } from "../dtos/mapper";

export async function signInWithEmailAndPassword(email: string, password: string): Promise<{
    accessToken: string;
    refreshToken: string;
    user: UserDto;
}> {
    const user = await usersRepository.findOne({
        where: { email },
        select: {
            id: true,
            email: true,
            role: true,
            emailConfirmed: true,
            lockoutEnd: true,
            hashedPassword: true,
        },
    });

    if (!user) {
        throw new AppError("Invalid credentials", 401);
    }
    if (user.hashedPassword === null) {
        throw new AppError(
            "Your account is not registered with email and password. Try using another sign in method or reset your password",
            401
        );
    }

    const isMatchPassword = await bcrypt.compare(password, user.hashedPassword);

    if (!isMatchPassword) {
        throw new AppError("Invalid credentials", 401);
    }

    if (user.isLocked()) {
        throw new AppError("You are locked out!", 403);
    }

    const accessToken = jwt.signForUser(user);
    const refreshToken = await refreshTokensRepository.generateRefreshTokenForUser(user.id);

    logger.info("User logged in", {
        email: user.email,
    });

    return {
        user: mapUserEntityToUserDto(user),
        accessToken,
        refreshToken,
    };
}

export async function signInWithGoogle(idToken: string): Promise<{
    accessToken: string;
    refreshToken: string;
    user: UserDto;
}> {
    const client = new OAuth2Client(Config.Google.ClientID);
    let payload: TokenPayload | undefined;

    try {
        const ticket = await client.verifyIdToken({
            idToken: idToken,
            audience: Config.Google.ClientID,
        });
        payload = ticket.getPayload();
    } catch (error) {
        throw new AppError("Invalid credentials", 401);
    }

    if (!payload?.email) {
        throw new AppError("Invalid credentials", 401);
    }

    let user = await usersRepository.findOne({
        where: { email: payload.email },
        select: {
            id: true,
            email: true,
            role: true,
            emailConfirmed: true,
            lockoutEnd: true,
        },
    });

    let accessToken = "";
    let refreshToken = "";
    let userDto = undefined;

    if (user) {
        if (user.isLocked()) {
            throw new AppError("You are locked out!", 403);
        }

        if (!user.emailConfirmed) {
            await usersRepository.update(user.id, {
                emailConfirmed: true,
                emailConfirmationToken: null,
                emailConfirmationTokenExpiryTime: null,
            });
            user.emailConfirmed = true;
        }

        accessToken = jwt.signForUser(user);
        refreshToken = await refreshTokensRepository.generateRefreshTokenForUser(user.id);
        userDto = mapUserEntityToUserDto(user);

        logger.info("User logged in with google account", {
            email: user.email,
        });
    } else {
        user = usersRepository.create({
            name: payload.name,
            email: payload.email,
            emailConfirmed: true,
        });

        await usersRepository.insert(user);

        userDto = mapUserEntityToUserDto(user);

        await rabbitmq.publishUserCreatedEvent(userDto);

        logger.info(`New user registered with google account`, {
            id: user.id,
            email: user.email,
        });

        accessToken = jwt.signForUser(user);
        refreshToken = await refreshTokensRepository.generateRefreshTokenForUser(user.id);
    }

    return {
        accessToken,
        refreshToken,
        user: userDto,
    };
}

export async function refreshAccessToken(userId: string, refreshToken: string): Promise<string> {
    const user = await usersRepository.findOne({
        where: { id: userId },
        select: {
            id: true,
            email: true,
            role: true,
            emailConfirmed: true,
            lockoutEnd: true,
        },
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    if (user.isLocked()) {
        throw new AppError("You are locked out!", 403);
    }

    const token = await refreshTokensRepository.findOne({
        where: {
            userId,
            token: refreshToken,
        },
    });

    if (!token || token.expiryTime < new Date()) {
        throw new AppError("Invalid token", 403);
    }

    const accessToken = jwt.signForUser(user);

    return accessToken;
}

export async function revokeRefreshToken(userId: string, refreshToken: string) {
    await refreshTokensRepository.delete({
        userId,
        token: refreshToken,
    });
}
