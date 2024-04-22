import { tokenRepository } from "../data/token.repository";
import { userRepository } from "../data/user.repository";
import { TokenType } from "../entity/token.entity";
import { AppError } from "../utils/app-error";
import * as rabbitmq from "../rabbitmq/client";
import Config from "../config";

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

export async function sendConfirmEmailLink(email: string) {
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
            user: {
                id: user.id,
                email: user.email,
                name: user.name,
            },
            link,
            emailType: "confirm_email",
        });
    }
}
