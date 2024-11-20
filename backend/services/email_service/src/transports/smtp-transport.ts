import * as nodemailer from "nodemailer";
import Config from "../config";

export function createSmtpTransport() {
    return nodemailer.createTransport({
        host: Config.SMTP_HOST,
        port: Config.SMTP_PORT,
        secure: false,
        auth: {
            user: Config.SMTP_USER,
            pass: Config.SMTP_PASS,
        },
    });
}
