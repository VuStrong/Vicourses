import * as bcrypt from "bcrypt";
import { tokenRepository } from "../data/token.repository";
import { userRepository } from "../data/user.repository";
import { AppError } from "../utils/app-error";
import { TokenType } from "../entity/token.entity";
import * as rabbitmq from "../rabbitmq/client";
import Config from "../config";

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

export async function sendResetPasswordLink(email: string) {
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
        userName: user.name,
        link,
        emailType: "reset_password"
    });
}
