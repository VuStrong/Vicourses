import dotenv from "dotenv";

dotenv.config();

const Config = {
    NODE_ENV: process.env.NODE_ENV || "development",
    PORT: Number(process.env.PORT) || 3000,

    S3_ENDPOINT: process.env.S3_ENDPOINT,
    S3_ACCOUNT_ID: process.env.S3_ACCOUNT_ID,
    S3_ACCESS_KEY_ID: process.env.S3_ACCESS_KEY_ID,
    S3_ACCESS_KEY_SECRET: process.env.S3_ACCESS_KEY_SECRET,
    S3_BUCKET_NAME: process.env.S3_BUCKET_NAME,
    S3_DOMAIN: process.env.S3_DOMAIN,

    RABBITMQ_URL: process.env.RABBITMQ_URL,
};

export default Config;