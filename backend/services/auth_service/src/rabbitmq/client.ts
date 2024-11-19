import amqplib, { Channel, Connection } from 'amqplib';
import User from '../entity/user.entity';
import Config from '../config';
import logger from '../logger';

let connection: Connection;
let channel: Channel;

export enum Queue {
    SEND_EMAIL = "send_email"
}

export enum Exchange {
    USER_CREATED = "user.created",
    USER_ROLE_UPDATED = "user.role.updated",
}

export async function connect() {
    try {
        connection = await amqplib.connect(Config.RABBITMQ_URL ?? "");
        channel = await connection.createChannel();

        await channel.assertQueue(Queue.SEND_EMAIL);

        await channel.assertExchange(Exchange.USER_CREATED, "fanout");
        await channel.assertExchange(Exchange.USER_ROLE_UPDATED, "fanout");

        logger.info("Connected to Rabbitmq");
    } catch (error: any) {
        logger.error(`Error while connecting to RabbitMQ: ${error.code}`);
    }
}

export function sendToQueue(queue: Queue, message: any) {
    if (!channel) return;

    channel.sendToQueue(queue, Buffer.from(JSON.stringify(message)));
}

export function publishNewCreatedUser(user: User) {
    if (!channel) return;

    channel.publish(Exchange.USER_CREATED, "", Buffer.from(JSON.stringify(user)));
}

export function publishUserRoleUpdated(user: User) {
    if (!channel) return;
    
    channel.publish(Exchange.USER_ROLE_UPDATED, "", Buffer.from(JSON.stringify({
        id: user.id,
        role: user.role,
    })));
}