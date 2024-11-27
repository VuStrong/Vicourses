import dotenv from "dotenv";

dotenv.config();

const Config = {
    Environment: process.env.NODE_ENV || "development",
    Port: Number(process.env.PORT) || 3000,

    S3: {
        Endpoint: process.env.S3_ENDPOINT,
        AccountId: process.env.S3_ACCOUNT_ID,
        AccessKeyId: process.env.S3_ACCESS_KEY_ID,
        AccessKeySecret: process.env.S3_ACCESS_KEY_SECRET,
        BucketName: process.env.S3_BUCKET_NAME,
        Domain: process.env.S3_DOMAIN,
    },

    RabbitMQ: {
        Url: process.env.RABBITMQ_URL,
        RetryDelay: Number(process.env.RABBITMQ_RETRY_DELAY) || 0,
    },

    FileUploadSecret: process.env.FILE_UPLOAD_SECRET,
};

export default Config;