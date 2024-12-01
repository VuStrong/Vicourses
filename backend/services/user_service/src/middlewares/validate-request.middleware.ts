import { Request, Response, NextFunction } from "express";
import Joi, { ObjectSchema } from "joi";
import { Role } from "../entities/user.entity";

type RouteName =
    | "/auth/login"
    | "/auth/register"
    | "/auth/refresh-token"
    | "/auth/revoke-refresh-token"
    | "/auth/confirm-email"
    | "/auth/email-confirmation-link"
    | "/auth/reset-password"
    | "/auth/password-reset-link"
    | "/auth/google-login"
    
    | "(patch)/me"
    | "(patch)/me/password"
    | "(post)/me/paypal"

    | "(get)/users"
    | "(patch)/users/:id/lock";

type ValidateFrom = "body" | "query";

export default function validateRequest(route: RouteName, from: ValidateFrom = "body") {
    return function (req: Request, res: Response, next: NextFunction) {
        const schema = schemas[route];

        const validationResult = schema.validate(req[from], {
            allowUnknown: true,
        });

        if (validationResult.error) {
            const errors = validationResult.error.details.map((e) => e.message);

            return res.status(400).send({
                message: "Validation failed",
                code: 400,
                errors,
            });
        }

        next();
    };
}

const schemas: {
    [key in RouteName]: ObjectSchema;
} = {
    "/auth/login": Joi.object({
        email: Joi.string().email().required(),
        password: Joi.string().required(),
    }),

    "/auth/register": Joi.object({
        name: Joi.string().min(2).max(50).required(),
        email: Joi.string().email().required(),
        password: Joi.string().min(8).max(50).required(),
    }),

    "/auth/refresh-token": Joi.object({
        refreshToken: Joi.string().required(),
        userId: Joi.string().required(),
    }),

    "/auth/revoke-refresh-token": Joi.object({
        refreshToken: Joi.string().required(),
        userId: Joi.string().required(),
    }),

    "/auth/confirm-email": Joi.object({
        token: Joi.string().required(),
        userId: Joi.string().required(),
    }),

    "/auth/email-confirmation-link": Joi.object({
        email: Joi.string().email().required(),
    }),

    "/auth/reset-password": Joi.object({
        token: Joi.string().required(),
        userId: Joi.string().required(),
        newPassword: Joi.string().min(8).max(50).required(),
    }),

    "/auth/password-reset-link": Joi.object({
        email: Joi.string().email().required(),
    }),

    "/auth/google-login": Joi.object({
        idToken: Joi.string().required(),
    }),

    "(patch)/me": Joi.object({
        name: Joi.string().min(2).max(50).optional(),
        thumbnailToken: Joi.string().optional(),
        headline: Joi.string().max(60).optional(),
        description: Joi.string().optional(),
        websiteUrl: Joi.string().uri().optional(),
        youtubeUrl: Joi.string().uri().optional(),
        facebookUrl: Joi.string().uri().optional(),
        linkedInUrl: Joi.string().uri().optional(),
        enrolledCoursesVisible: Joi.boolean().optional(),
        isPublic: Joi.boolean().optional(),
        categoryIds: Joi.string().optional(),
    }),

    "(patch)/me/password": Joi.object({
        oldPassword: Joi.string().required(),
        newPassword: Joi.string().min(8).max(50).required(),
    }),

    "(post)/me/paypal": Joi.object({
        code: Joi.string().required(),
    }),

    "(get)/users": Joi.object({
        skip: Joi.number().integer().optional(),
        limit: Joi.number().integer().max(100).optional(),
        role: Joi.string().allow(Role.ADMIN, Role.INSTRUCTOR, Role.STUDENT).optional(),
        order: Joi.string().regex(/^\w+(_desc|_asc)+$/).optional(),
    }),

    "(patch)/users/:id/lock": Joi.object({
        days: Joi.number().integer().required(),
    }),
};
