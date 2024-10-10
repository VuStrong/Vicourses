import amqplib, { Channel, Connection } from 'amqplib';
import Config from '../config';
import { deleteMultipleFiles } from '../services/s3-upload.service';

let connection: Connection;
let channel: Channel;

enum Queue {
    DELETE_FILES = "delete_files"
}

export async function connect() {
    try {
        connection = await amqplib.connect(Config.RABBITMQ_URL ?? "");
        channel = await connection.createChannel();

        console.log("Connected to Rabbitmq");

        await consumeDeleteFilesEvent();
    } catch (error: any) {
        console.log(`Error while connecting to RabbitMQ: ${error.message}`);
    }
}

async function consumeDeleteFilesEvent() {
    const q = await channel.assertQueue(Queue.DELETE_FILES);
    
    await channel.consume(q.queue, async (msg) => {
        if (!msg?.content) return;

        const content = JSON.parse(msg.content.toString());

        if (Array.isArray(content.fileIds)) {
            try {
                await deleteMultipleFiles(content.fileIds);
            } catch (error: any) {
                console.log(`Error when process delete files event: ${error.message}`);
            }            
        }
    }, {
        noAck: true,
    });
}