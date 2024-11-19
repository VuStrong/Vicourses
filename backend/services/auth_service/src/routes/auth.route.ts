import { Router } from "express";
import * as authController from "../controllers/auth.controller";
import {
    checkValidationResult,
    validateConfirmEmailRequest,
    validateGoogleLogin,
    validateLogin,
    validateRefreshTokenRequest,
    validateRegister,
    validateResendEmailConfirmationRequest,
    validateResetPasswordRequest,
    validateSendPasswordResetLinkRequest,
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
    "/email-confirmation-link",
    validateResendEmailConfirmationRequest(),
    checkValidationResult,
    authController.handleSendEmailConfirmationLink
);

authRoute.post(
    "/reset-password",
    validateResetPasswordRequest(),
    checkValidationResult,
    authController.handleResetPassword
);
authRoute.post(
    "/password-reset-link",
    validateSendPasswordResetLinkRequest(),
    checkValidationResult,
    authController.handleSendPasswordResetLink
);

authRoute.post(
    "/google-login",
    validateGoogleLogin(),
    checkValidationResult,
    authController.handleGoogleLogin
);

export default authRoute;
