import amqplib, { Connection } from "amqplib";
import Config from "../config";
import { sendEmail } from "../services/email.service";
import logger from "../logger";

let connection: Connection;

export async function connectAndConsume() {
    try {
        connection = await amqplib.connect(Config.RABBITMQ_URI ?? "");

        logger.info("Connected to Rabbitmq");

        await startConsume();
    } catch (error: any) {
        logger.error(`Error while connecting to RabbitMQ: ${error.code}`);
    }
}

async function startConsume() {
    const channel = await connection.createChannel();

    const queue = await channel.assertQueue("send_email");

    await channel.consume(
        queue.queue,
        async (msg) => {
            if (!msg?.content) return;
            
            try {
                const data = JSON.parse(msg.content.toString());

                await sendEmail(data);

                logger.info(`Email has been sent to the user: ${data.to}`);
            } catch (error: any) {
                logger.error(`Error sending email: ${error.message}`);
            }
        },
        {
            noAck: true,
        }
    );
}
