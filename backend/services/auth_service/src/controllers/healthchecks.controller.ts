import { NextFunction, Request, Response } from "express";
import mysql from "mysql2";
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

export async function handleHealthCheck(req: Request, res: Response, next: NextFunction) {
    const healthcheck: HealthChecksResponse = {
        uptime: process.uptime(),
        status: 'Healthy',
        timestamp: Date.now(),
        entries: {}
    };

    try {
        await checkMysql();

        healthcheck.entries.mysql = {
            status: "Healthy",
        }
    } catch (err: any) {
        healthcheck.status = "Unhealthy";
        healthcheck.entries.mysql = {
            status: "Unhealthy",
            description: err.message,
            exception: err.message,
        }
    }

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
        res.send(healthcheck);
    } catch (error: any) {
        healthcheck.status = "Unhealthy";
        healthcheck.description = error.message;
        healthcheck.exception = error.message;

        res.status(503).send(healthcheck);
    }
}

async function checkMysql(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
        const connection = mysql.createConnection({
            host: Config.DB_HOST,
            port: Config.DB_PORT,
            database: Config.DB_DATABASE,
            user: Config.DB_USER,
            password: Config.DB_PASS,
        });
    
        connection.connect(function (err) {
            if (err) {
                reject(new Error(err.message));                
            } else {
                resolve();
            }
        });  
    });
}

async function checkRabbitMQ(): Promise<void> {
    try {
        await amqplib.connect(Config.RABBITMQ_URL ?? "");
    } catch (error: any) {
        throw new Error(error.code);
    }
}