import { UserDto } from "../dtos/user-dtos";
import { getPublisherChannel } from "./connection";

export async function sendToEmailQueue(message: {
    to: string
    template: "confirm_email" | "reset_password",
    payload: any,
}) {
    const channel = getPublisherChannel();
    if (!channel) return;

    const queue = "send_email";

    await channel.assertQueue(queue);

    channel.sendToQueue(queue, Buffer.from(JSON.stringify(message)));
}

export async function publishUserCreatedEvent(user: UserDto) {
    const channel = getPublisherChannel();
    if (!channel) return;

    const exchange = "user.created";

    await channel.assertExchange(exchange, "fanout");

    channel.publish(exchange, "", Buffer.from(JSON.stringify(user)));
}

export async function publishUserRoleUpdatedEvent(user: {
    id: string,
    role: string,
}) {
    const channel = getPublisherChannel();
    if (!channel) return;

    const exchange = "user.role.updated";

    await channel.assertExchange(exchange, "fanout");

    channel.publish(exchange, "", Buffer.from(JSON.stringify(user)));
}

export async function publishUserInfoUpdatedEvent(user: UserDto) {
    const channel = getPublisherChannel();
    if (!channel) return;
    
    const exchange = "user.info.updated";

    await channel.assertExchange(exchange, "fanout");

    channel.publish(exchange, "", Buffer.from(JSON.stringify(user)));
}

export async function publishUserPaypalAccountUpdated(user: {
    id: string,
    paypalAccount: {
        payerId: string,
        email: string,
        refreshToken: string,
    }
}) {
    const channel = getPublisherChannel();
    if (!channel) return;
    
    const exchange = "user.paypalaccount.updated";

    await channel.assertExchange(exchange, "fanout");

    channel.publish(exchange, "", Buffer.from(JSON.stringify(user)));
}