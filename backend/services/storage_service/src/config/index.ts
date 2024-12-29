import dotenv from "dotenv";

dotenv.config();

const Config = {
    Environment: process.env.NODE_ENV || "development",
    Port: Number(process.env.PORT) || 3000,

    AWS: {
        AccessKey: process.env.AWS_ACCESS_KEY,
        SecretKey: process.env.AWS_SECRET_KEY,
        Region: process.env.AWS_REGION,
    },

    S3: {
        BucketName: process.env.S3_BUCKET_NAME,
    },

    Cloudfront: {
        Domain: process.env.CLOUDFRONT_DOMAIN,
        KeyPairId: process.env.CLOUDFRONT_KEYPAIR_ID,
        PrivateKeyPath: process.env.CLOUDFRONT_PRIVATE_KEY_PATH,
    },

    RabbitMQ: {
        Url: process.env.RABBITMQ_URL,
        RetryDelay: Number(process.env.RABBITMQ_RETRY_DELAY) || 0,
    },

    FileUploadSecret: process.env.FILE_UPLOAD_SECRET,
    MediaFileSecret: process.env.MEDIA_FILE_SECRET,
};

export default Config;