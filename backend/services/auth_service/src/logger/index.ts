import winston from "winston";
import "winston-daily-rotate-file";

const logger = winston.createLogger({
    format: winston.format.combine(
        winston.format.label({
            label: "Auth Service",
            message: true,
        }),
        winston.format.timestamp(),
        winston.format.printf((info) => {
            return JSON.stringify(info);
        })
    ),
    transports: [
        new winston.transports.Console({
            format: winston.format.prettyPrint(),
        }),
        new winston.transports.DailyRotateFile({
            filename: "logs/auth-service-%DATE%.log",
            datePattern: "YYYY-MM-DD",
            maxSize: 2242880,
            maxFiles: "2d",
        }),
    ],
});

export default logger;
