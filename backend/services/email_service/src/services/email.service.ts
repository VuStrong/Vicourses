import Joi from "joi";
import fs from "fs";
import ejs from "ejs";
import juice from "juice";
import {
    confirmEmailSchema,
    courseApprovedEmailSchema,
    courseNotApprovedEmailSchema,
    paymentCompletedEmailSchema,
    resetPasswordEmailSchema,
} from "../schemas/email-schemas";
import { createSmtpTransport } from "../transports/smtp-transport";
import Config from "../config";

type EmailTemplate =
    | "confirm_email"
    | "reset_password"
    | "course_approved"
    | "course_not_approved"
    | "payment_completed";

const sendEmailSchema = Joi.object({
    to: Joi.string().email().required(),
    template: Joi.string().required(),
    payload: Joi.object().optional(),
});

export type SendEmailInput = {
    to: string;
    template: EmailTemplate;
    payload?: {
        [key: string]: any;
    };
};

const transport = createSmtpTransport();

export async function sendEmail(input: SendEmailInput) {
    const subject = validateSendEmailInput(input);

    // Assign some default config
    input.payload ??= {};
    input.payload.appName = Config.APP_NAME;
    input.payload.appLogoUrl = Config.APP_LOGO_URL;
    input.payload.webUrl = Config.WEB_URL;

    const templatePath = `templates/${input.template}.ejs`;

    if (fs.existsSync(templatePath)) {
        const template = fs.readFileSync(templatePath, "utf-8");
        const html = ejs.render(template, input.payload);
        const htmlWithStylesInlined = juice(html);

        await transport.sendMail({
            from: Config.SMTP_USER,
            to: input.to,
            html: htmlWithStylesInlined,
            subject,
        });
    } else {
        throw new Error(`Template ${templatePath} not found`);
    }
}

function validateSendEmailInput(input: SendEmailInput): string {
    let validationResult: Joi.ValidationResult<any>;

    validationResult = sendEmailSchema.validate(input);

    if (validationResult.error) {
        throw new Error(validationResult.error.message);
    }

    let subject = "";

    switch (input.template) {
        case "confirm_email":
            validationResult = confirmEmailSchema.validate(input.payload, {
                allowUnknown: true,
            });
            subject = "Confirm your account";
            break;
        case "reset_password":
            validationResult = resetPasswordEmailSchema.validate(
                input.payload,
                { allowUnknown: true }
            );
            subject = "Reset password";
            break;
        case "course_approved":
            validationResult = courseApprovedEmailSchema.validate(
                input.payload,
                { allowUnknown: true }
            );
            subject = "Your course is approved: " + input.payload?.courseName;
            break;
        case "course_not_approved":
            validationResult = courseNotApprovedEmailSchema.validate(
                input.payload,
                { allowUnknown: true }
            );
            subject = "Your course is declined: " + input.payload?.courseName;
            break;
        case "payment_completed":
            validationResult = paymentCompletedEmailSchema.validate(
                input.payload,
                { allowUnknown: true }
            );
            subject = `You're in! Payment comfirmation for "${input.payload?.courseName}"`;
            break;
        default:
            throw new Error(`Template ${input.template} is not available`);
    }

    if (validationResult.error) {
        throw new Error(validationResult.error.message);
    }

    return subject;
}
