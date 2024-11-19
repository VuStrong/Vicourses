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
        body("name")
            .isString()
            .withMessage("name must be a string")
            .isLength({ min: 2, max: 50 })
            .withMessage("name must be between 2 and 50 characters")
            .isAlphanumeric("vi-VN", {
                ignore: " ",
            })
            .withMessage("name must be Alphanumeric"),
        body("email")
            .isEmail()
            .withMessage("email is invalid"),
        body("password")
            .isString()
            .withMessage("password must be a string")
            .isLength({ min: 8, max: 50 })
            .withMessage("password must be between 8 and 50 characters"),
    ];
};

export const validateLogin = () => {
    return [
        body("email")
            .isEmail()
            .withMessage("email is invalid"),
        body("password")
            .isString()
            .withMessage("password must be a string")
            .notEmpty()
            .withMessage("password must not empty"),
    ];
};

export const validateRefreshTokenRequest = () => {
    return [
        body("refreshToken")
            .isString()
            .withMessage("refreshToken must be a string")
            .notEmpty()
            .withMessage("refreshToken must not empty"),
        body("userId")
            .isString()
            .withMessage("userId must be a string")
            .notEmpty()
            .withMessage("userId must not empty"),
    ];
};

export const validateConfirmEmailRequest = () => {
    return [
        body("token")
            .isString()
            .withMessage("token must be a string")
            .notEmpty()
            .withMessage("token must not empty"),
        body("userId")
            .isString()
            .withMessage("userId must be a string")
            .notEmpty()
            .withMessage("userId must not empty"),
    ];
};

export const validateResendEmailConfirmationRequest = () => {
    return [
        body("email")
            .isEmail()
            .withMessage("email is invalid"),
    ];
};

export const validateSendPasswordResetLinkRequest = () => {
    return [
        body("email")
            .isEmail()
            .withMessage("email is invalid"),
    ];
};

export const validateResetPasswordRequest = () => {
    return [
        body("token")
            .isString()
            .withMessage("token must be a string")
            .notEmpty()
            .withMessage("token must not empty"),
        body("userId")
            .isString()
            .withMessage("userId must be a string")
            .notEmpty()
            .withMessage("userId must not empty"),
        body("newPassword")
            .isString()
            .withMessage("newPassword must be a string")
            .isLength({ min: 8, max: 50 })
            .withMessage("newPassword must be between 8 and 50 characters"),
    ];
};

export const validateGoogleLogin = () => {
    return [
        body("idToken")
            .isString()
            .withMessage("idToken must be a string")
            .notEmpty()
            .withMessage("idToken must not empty"),
    ];
};

export const validateChangePasswordRequest = () => {
    return [
        body("oldPassword")
            .isString()
            .not().isEmpty()
            .withMessage('oldPassword must not empty'),
        body("newPassword")
            .isString()
            .withMessage("newPassword must be a string")
            .isLength({ min: 8, max: 50 })
            .withMessage("newPassword must be between 8 and 50 characters"),
    ];
};