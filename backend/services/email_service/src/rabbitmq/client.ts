import amqplib, { Channel, Connection } from "amqplib";
import Config from "../config";
import { sendEmail } from "../services/email.service";
import logger from "../logger";

let connection: Connection | null = null;
let channel: Channel | null = null;

export async function connectAndConsume() {
    try {
        connection = await amqplib.connect(Config.RabbitMQ.Uri ?? "");
        channel = await connection.createChannel();

        logger.info("Connected to Rabbitmq");

        connection.on("close", onConnectionClose);

        await startConsume();
    } catch (error: any) {
        logger.error(`Error connecting to RabbitMQ: ${error.code}`);

        if (Config.RabbitMQ.RetryDelay > 0) {
            setTimeout(connectAndConsume, Config.RabbitMQ.RetryDelay * 1000);
        }
    }
}

function onConnectionClose(error: any) {
    console.error(`RabbitMQ connection closed, error: ${error.message}`);

    connection?.removeAllListeners();
    connection = null;

    if (channel) {
        channel = null;
    }
    
    if (Config.RabbitMQ.RetryDelay > 0) {
        setTimeout(connectAndConsume, Config.RabbitMQ.RetryDelay * 1000);
    } 
}

async function startConsume() {
    if (!channel) return;

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
