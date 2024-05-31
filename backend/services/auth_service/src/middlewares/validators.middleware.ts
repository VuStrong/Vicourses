import { NextFunction, Request, Response } from "express";
import { check, validationResult } from "express-validator";

export const checkValidationResult = (req: Request, res: Response, next: NextFunction) => {
    const errors = validationResult(req);

    if (!errors.isEmpty()) {
        res.status(400).json({ 
            message: "Validation failed",
            errors: errors.array().map((e) => e.msg) 
        });
        return;
    }

    next();
};

export const validateRegister = () => {
    return [
        check("name", "name must not empty").not().isEmpty(),
        check("name", "name must be Alphanumeric").isAlphanumeric("vi-VN", {
            ignore: " ",
        }),
        check("email", "email must not empty").not().isEmpty(),
        check("email", "Invalid email").isEmail(),
        check("password", "password must not less than 8 characters").isLength({
            min: 8,
        }),
    ];
};

export const validateLogin = () => {
    return [
        check("email", "email must not empty").not().isEmpty(),
        check("email", "Invalid email").isEmail(),
        check("password", "password must not empty").not().isEmpty(),
    ];
};

export const validateRefreshTokenRequest = () => {
    return [
        check("refreshToken", "refreshToken must not empty").not().isEmpty(),
        check("userId", "userId must not empty").not().isEmpty(),
    ];
};

export const validateConfirmEmailRequest = () => {
    return [
        check("token", "token must not empty").not().isEmpty(),
        check("userId", "userId must not empty").not().isEmpty(),
    ];
};

export const validateResendConfirmEmailRequest = () => {
    return [
        check("email", "email must not empty").not().isEmpty(),
        check("email", "Invalid email").isEmail(),
    ];
};

export const validateSendResetPasswordLinkRequest = () => {
    return [
        check("email", "email must not empty").not().isEmpty(),
        check("email", "Invalid email").isEmail(),
    ];
};

export const validateResetPasswordRequest = () => {
    return [
        check("token", "token must not empty").not().isEmpty(),
        check("userId", "userId must not empty").not().isEmpty(),
        check(
            "newPassword",
            "newPassword must not less than 8 characters"
        ).isLength({ min: 8 }),
    ];
};
