import dotenv from "dotenv";

dotenv.config();

const Config = {
    NODE_ENV: process.env.NODE_ENV || "development",
    PORT: Number(process.env.PORT) || 3000,

    WEB_CLIENT_EMAIL_CONFIRM_URL: process.env.WEB_CLIENT_EMAIL_CONFIRM_URL,
    WEB_CLIENT_RESET_PASSWORD_URL: process.env.WEB_CLIENT_RESET_PASSWORD_URL,
    
    DB_HOST: process.env.DB_HOST,
    DB_PORT: Number(process.env.DB_PORT) || 3306,
    DB_USER: process.env.DB_USER,
    DB_PASS: process.env.DB_PASS,
    DB_DATABASE: process.env.DB_DATABASE,

    RABBITMQ_URL: process.env.RABBITMQ_URL,

    GOOGLE_CLIENT_ID: process.env.GOOGLE_CLIENT_ID,
    GOOGLE_CLIENT_SECRET: process.env.GOOGLE_CLIENT_SECRET,

    JWT_LIFETIME: "30m", // 30 minutes
    REFRESH_TOKEN_LIFETIME: 60, // 60 days
    EMAIL_CONFIRMATION_TOKEN_LIFETIME: 1, // 1 days
    RESET_PASSWORD_TOKEN_LIFETIME: 1, // 1 days
}

export default Config;