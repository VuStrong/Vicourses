import dotenv from "dotenv";

dotenv.config();

const Config = {
    PORT: Number(process.env.PORT) || 3000,

    SMTP_HOST: process.env.SMTP_HOST,
    SMTP_PORT: Number(process.env.SMTP_PORT) || 587,
    SMTP_USER: process.env.SMTP_USER,
    SMTP_PASS: process.env.SMTP_PASS,

    RABBITMQ_URI: process.env.RABBITMQ_URI,

    APP_NAME: process.env.APP_NAME || "Vicourses",
    APP_LOGO_URL: process.env.APP_LOGO_URL,
    WEB_URL: process.env.WEB_URL,
}

export default Config;