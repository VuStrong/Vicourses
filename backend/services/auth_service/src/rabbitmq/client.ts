import amqplib, { Channel, Connection } from 'amqplib';
import User from '../entity/user.entity';
import Config from '../config';

let connection: Connection;
let channel: Channel;

export enum Queue {
    SEND_EMAIL = "send_email"
}

export enum Exchange {
    USER_CREATED = "user.created"
}

export async function connect() {
    try {
        connection = await amqplib.connect(Config.RABBITMQ_URL ?? "");
        channel = await connection.createChannel();

        await channel.assertQueue(Queue.SEND_EMAIL);

        await channel.assertExchange(Exchange.USER_CREATED, "fanout");

        console.log("Connected to Rabbitmq");
    } catch (error) {
        console.log(`Error while connecting to RabbitMQ: ${error}`);
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