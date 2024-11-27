import amqplib, { Channel, Connection } from 'amqplib';
import Config from '../config';
import logger from '../logger';
import handleUserEnrolledEvent from '../events/user-enrolled.event';

let connection: Connection | null = null;
let consumerChannel: Channel | null = null;
let publisherChannel: Channel | null = null;

export function getPublisherChannel() {
    return publisherChannel;
}

export async function connect() {
    try {
        connection = await amqplib.connect(Config.RabbitMQ.Url ?? "");
        consumerChannel = await connection.createChannel();
        publisherChannel = await connection.createChannel();

        logger.info("Connected to Rabbitmq");

        connection.on("close", onConnectionClose);

        await consumeUserEnrolledEvent();
    } catch (error: any) {
        logger.error(`Error connecting to RabbitMQ: ${error.code}`);

        if (Config.RabbitMQ.RetryDelay > 0) {
            setTimeout(connect, Config.RabbitMQ.RetryDelay * 1000);
        } 
    }
}

function onConnectionClose(error: any) {
    logger.error(`RabbitMQ connection closed, error: ${error.message}`);

    connection?.removeAllListeners();
    connection = null;

    if (consumerChannel) {
        consumerChannel = null;
    }
    if (publisherChannel) {
        publisherChannel = null;
    }
    
    if (Config.RabbitMQ.RetryDelay > 0) {
        setTimeout(connect, Config.RabbitMQ.RetryDelay * 1000);
    } 
}

async function consumeUserEnrolledEvent() {
    if (!consumerChannel) return;

    const exchange = "user.enrolled";

    await consumerChannel.assertExchange(exchange, "fanout");

    const queue = await consumerChannel.assertQueue("user_service.user.enrolled");

    await consumerChannel.bindQueue(queue.queue, exchange, "");

    await consumerChannel.consume(
        queue.queue,
        async (msg) => {
            if (!msg?.content) return;

            try {
                const data = JSON.parse(msg.content.toString());

                await handleUserEnrolledEvent(data);
            } catch (error: any) {
                logger.error(`Error process user.enrolled event: ${error.message}`);
            }
        },
        {
            noAck: true,
        }
    );
}