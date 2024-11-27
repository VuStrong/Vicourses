import amqplib, { Channel, Connection } from 'amqplib';
import Config from '../config';
import { deleteMultipleFiles } from '../services/s3-upload.service';

let connection: Connection | null = null;
let channel: Channel | null = null;

export async function connect() {
    try {
        connection = await amqplib.connect(Config.RabbitMQ.Url ?? "");
        channel = await connection.createChannel();

        console.log("Connected to Rabbitmq");

        connection.on("close", onConnectionClose);

        await consumeDeleteFilesEvent();
    } catch (error: any) {
        console.error(`Error connecting to RabbitMQ: ${error.code}`);

        if (Config.RabbitMQ.RetryDelay > 0) {
            setTimeout(connect, Config.RabbitMQ.RetryDelay * 1000);
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
        setTimeout(connect, Config.RabbitMQ.RetryDelay * 1000);
    } 
}

async function consumeDeleteFilesEvent() {
    if (!channel) return;

    const q = await channel.assertQueue("delete_files");
    
    await channel.consume(q.queue, async (msg) => {
        if (!msg?.content) return;

        const content = JSON.parse(msg.content.toString());

        if (Array.isArray(content.fileIds)) {
            try {
                await deleteMultipleFiles(content.fileIds);
            } catch (error: any) {
                console.error(`Error when process delete files event: ${error.message}`);
            }
        }
    }, {
        noAck: true,
    });
}