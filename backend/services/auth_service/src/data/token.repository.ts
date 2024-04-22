import Config from "../config";
import Token, { TokenType } from "../entity/token.entity";
import { generateRandomString } from "../utils/random";
import { dataSource } from "./data-source";

export const tokenRepository = dataSource.getRepository(Token).extend({
    async generateRefreshToken(userId: string) {
        const refreshToken = generateRandomString(50);
        const date = new Date();
        date.setDate(date.getDate() + Config.REFRESH_TOKEN_LIFETIME);
        
        const token = this.create({
            token: refreshToken,
            userId,
            type: TokenType.REFRESH_TOKEN,
            expiryTime: date
        });

        await this.save(token);

        return refreshToken;
    },
    async generateConfirmEmailToken(userId: string) {
        await this.delete({
            userId,
            type: TokenType.CONFIRM_EMAIL
        });

        const confirmEmailToken = generateRandomString(50);
        const date = new Date();
        date.setDate(date.getDate() + Config.EMAIL_CONFIRMATION_TOKEN_LIFETIME);
        
        const token = this.create({
            token: confirmEmailToken,
            userId,
            type: TokenType.CONFIRM_EMAIL,
            expiryTime: date
        });

        await this.save(token);

        return confirmEmailToken;
    },
    async generateResetPasswordToken(userId: string) {
        await this.delete({
            userId,
            type: TokenType.RESET_PASSWORD
        });

        const resetPassToken = generateRandomString(50);
        const date = new Date();
        date.setDate(date.getDate() + Config.RESET_PASSWORD_TOKEN_LIFETIME);
        
        const token = this.create({
            token: resetPassToken,
            userId,
            type: TokenType.RESET_PASSWORD,
            expiryTime: date
        });

        await this.save(token);

        return resetPassToken;
    },
})