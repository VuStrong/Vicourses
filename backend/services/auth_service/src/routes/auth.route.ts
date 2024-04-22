import { Router } from "express";
import * as authController from "../controllers/auth.controller";
import {
    checkValidationResult,
    validateConfirmEmailRequest,
    validateLogin,
    validateRefreshTokenRequest,
    validateRegister,
    validateResendConfirmEmailRequest,
    validateResetPasswordRequest,
    validateSendResetPasswordLinkRequest,
} from "../middlewares/validators.middleware";

const authRoute = Router();

authRoute.post(
    "/login",
    validateLogin(),
    checkValidationResult,
    authController.handleLogin
);

authRoute.post(
    "/register",
    validateRegister(),
    checkValidationResult,
    authController.handleRegister
);

authRoute.post(
    "/refresh-token",
    validateRefreshTokenRequest(),
    checkValidationResult,
    authController.handleRefreshToken
);
authRoute.post(
    "/revoke-refresh-token",
    validateRefreshTokenRequest(),
    checkValidationResult,
    authController.handleRevokeRefreshToken
);

authRoute.post(
    "/confirm-email",
    validateConfirmEmailRequest(),
    checkValidationResult,
    authController.handleConfirmEmail
);
authRoute.post(
    "/confirm-email-link",
    validateResendConfirmEmailRequest(),
    checkValidationResult,
    authController.handleSendConfirmEmailLink
);

authRoute.post(
    "/reset-password",
    validateResetPasswordRequest(),
    checkValidationResult,
    authController.handleResetPassword
);
authRoute.post(
    "/reset-password-link",
    validateSendResetPasswordLinkRequest(),
    checkValidationResult,
    authController.handleSendResetPasswordLink
);

export default authRoute;
