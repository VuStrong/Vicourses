import * as nodemailer from "nodemailer";
import Config from "../config";

export function createSmtpTransport() {
    return nodemailer.createTransport({
        host: Config.Smtp.Host,
        port: Config.Smtp.Port,
        secure: false,
        auth: {
            user: Config.Smtp.User,
            pass: Config.Smtp.Password,
        },
    });
}
