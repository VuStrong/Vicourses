import { NextFunction, Request, Response } from "express";
import { body, validationResult } from "express-validator";

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
        body("name", "name must not empty").not().isEmpty(),
        body("name", "name must be Alphanumeric").isAlphanumeric("vi-VN", {
            ignore: " ",
        }),
        body("email", "email must not empty").not().isEmpty(),
        body("email", "Invalid email").isEmail(),
        body("password", "password must not less than 8 characters").isLength({
            min: 8,
        }),
    ];
};

export const validateLogin = () => {
    return [
        body("email", "email must not empty").not().isEmpty(),
        body("email", "Invalid email").isEmail(),
        body("password", "password must not empty").not().isEmpty(),
    ];
};

export const validateRefreshTokenRequest = () => {
    return [
        body("refreshToken", "refreshToken must not empty").not().isEmpty(),
        body("userId", "userId must not empty").not().isEmpty(),
    ];
};

export const validateConfirmEmailRequest = () => {
    return [
        body("token", "token must not empty").not().isEmpty(),
        body("userId", "userId must not empty").not().isEmpty(),
    ];
};

export const validateResendConfirmEmailRequest = () => {
    return [
        body("email", "email must not empty").not().isEmpty(),
        body("email", "Invalid email").isEmail(),
    ];
};

export const validateSendResetPasswordLinkRequest = () => {
    return [
        body("email", "email must not empty").not().isEmpty(),
        body("email", "Invalid email").isEmail(),
    ];
};

export const validateResetPasswordRequest = () => {
    return [
        body("token", "token must not empty").not().isEmpty(),
        body("userId", "userId must not empty").not().isEmpty(),
        body(
            "newPassword",
            "newPassword must not less than 8 characters"
        ).isLength({ min: 8 }),
    ];
};

export const validateGoogleLogin = () => {
    return [
        body("idToken", "idToken must not empty").not().isEmpty(),
    ];
};