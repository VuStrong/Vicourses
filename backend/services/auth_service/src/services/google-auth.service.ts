import { OAuth2Client, TokenPayload } from "google-auth-library";
import Config from "../config";
import User from "../entity/user.entity";
import { userRepository } from "../data/user.repository";
import { AppError } from "../utils/app-error";
import * as jwt from "../utils/jwt";
import { tokenRepository } from "../data/token.repository";
import logger from "../logger";
import { EventType, event } from "../events";

const client = new OAuth2Client(Config.GOOGLE_CLIENT_ID);

export async function googleLogin(idToken: string): Promise<{
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
        if (user.locked) {
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
