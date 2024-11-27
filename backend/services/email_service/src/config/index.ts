import dotenv from "dotenv";

dotenv.config();

const Config = {
    Port: Number(process.env.PORT) || 3000,

    Smtp: {
        Host: process.env.SMTP_HOST,
        Port: Number(process.env.SMTP_PORT) || 587,
        User: process.env.SMTP_USER,
        Password: process.env.SMTP_PASS,
    },

    RabbitMQ: {
        Uri: process.env.RABBITMQ_URI,
        RetryDelay: Number(process.env.RABBITMQ_RETRY_DELAY) || 0,
    },

    AppName: process.env.APP_NAME || "Vicourses",
    AppLogoUrl: process.env.APP_LOGO_URL,
    WebUrl: process.env.WEB_URL,
}

export default Config;