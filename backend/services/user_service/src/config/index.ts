import dotenv from "dotenv";

dotenv.config();

const Config = {
    Environment: process.env.NODE_ENV || "development",
    Port: Number(process.env.PORT) || 3000,

    WebUrls: {
        ConfirmEmail: process.env.WEB_CLIENT_EMAIL_CONFIRM_URL,
        ResetPassword: process.env.WEB_CLIENT_RESET_PASSWORD_URL,
    },

    Database: {
        Host: process.env.DB_HOST,
        Port: Number(process.env.DB_PORT) || 3306,
        User: process.env.DB_USER,
        Password: process.env.DB_PASS,
        Name: process.env.DB_DATABASE || "vicourses_user_db",
    },

    RabbitMQ: {
        Url: process.env.RABBITMQ_URL,
        RetryDelay: Number(process.env.RABBITMQ_RETRY_DELAY) || 0,
    },

    FileUploadSecret: process.env.FILE_UPLOAD_SECRET,

    Google: {
        ClientID: process.env.GOOGLE_CLIENT_ID,
        ClientSecret: process.env.GOOGLE_CLIENT_SECRET,
    },

    Paypal: {
        ClientID: process.env.PAYPAL_CLIENT_ID,
        ClientSecret: process.env.PAYPAL_CLIENT_SECRET,
        Base: process.env.PAYPAL_BASE,
    }
}

export default Config;