import Config from "../config";
import { tokenRepository } from "../data/token.repository";
import User from "../entity/user.entity";
import * as rabbitmq from "../rabbitmq/client";

export default async function onUserCreated(user: User) {
    if (!user.emailConfirmed) {
        const confirmEmailToken = await tokenRepository.generateConfirmEmailToken(user.id);
        
        const webUrl = Config.WEB_CLIENT_EMAIL_CONFIRM_URL;
        const link = `${webUrl}?token=${confirmEmailToken}&userId=${user.id}`;
    
        rabbitmq.sendToQueue(rabbitmq.Queue.SEND_EMAIL, {
            user: {
                id: user.id,
                email: user.email,
                name: user.name,
            },
            link,
            emailType: "confirm_email"
        });
    }

    rabbitmq.publishNewCreatedUser(user);
}