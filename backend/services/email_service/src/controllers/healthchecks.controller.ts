import { IncomingMessage, ServerResponse } from "http";
import amqplib from 'amqplib';
import Config from "../config";

type HealthChecksResponse = {
    status: "Healthy" | "Unhealthy";
    uptime: number;
    timestamp: number;
    exception?: string;
    description?: string;
    entries: {
        [key: string]: {
            status: "Healthy" | "Unhealthy";
            exception?: string;
            description?: string;
        }
    }
}

export async function handleHealthChecks(req: IncomingMessage, res: ServerResponse) {
    const healthcheck: HealthChecksResponse = {
        uptime: process.uptime(),
        status: 'Healthy',
        timestamp: Date.now(),
        entries: {}
    };

    try {
        await checkRabbitMQ();

        healthcheck.entries.rabbitmq = {
            status: "Healthy",
        }
    } catch (err: any) {
        healthcheck.status = "Unhealthy";
        healthcheck.entries.rabbitmq = {
            status: "Unhealthy",
            description: err.message,
            exception: err.message,
        }
    }

    try {
        res.writeHead(200, {
            "Content-Type": "application/json",
        });
        res.write(JSON.stringify(healthcheck));
        res.end();
    } catch (error: any) {
        healthcheck.status = "Unhealthy";
        healthcheck.description = error.message;
        healthcheck.exception = error.message;

        res.writeHead(503, {
            "Content-Type": "application/json",
        });
        res.write(JSON.stringify(healthcheck));
        res.end();
    }
}

async function checkRabbitMQ(): Promise<void> {
    try {
        await amqplib.connect(Config.RABBITMQ_URI ?? "");
    } catch (error: any) {
        throw new Error(error.code);
    }
}