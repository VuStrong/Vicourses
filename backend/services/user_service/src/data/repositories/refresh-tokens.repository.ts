import crypto from "crypto";
import RefreshToken from "../../entities/refresh-token.entity";
import { dataSource } from "../data-source";

export const refreshTokensRepository = dataSource.getRepository(RefreshToken).extend({
    async generateRefreshTokenForUser(userId: string) {
        const refreshToken = crypto.randomBytes(64).toString('hex');
        const date = new Date();
        date.setDate(date.getDate() + 60);
        
        const token = this.create({
            userId,
            token: refreshToken,
            expiryTime: date,
        });

        await this.insert(token);

        return refreshToken;
    },
})