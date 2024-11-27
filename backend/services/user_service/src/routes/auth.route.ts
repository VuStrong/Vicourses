import { Router } from "express";
import * as authController from "../controllers/auth.controller";
import validateRequest from "../middlewares/validate-request.middleware";

const authRoute = Router();

authRoute.post(
    "/auth/login",
    validateRequest("/auth/login"),
    authController.handleLogin
);

authRoute.post(
    "/auth/register",
    validateRequest("/auth/register"),
    authController.handleRegister
);

authRoute.post(
    "/auth/refresh-token",
    validateRequest("/auth/refresh-token"),
    authController.handleRefreshToken
);
authRoute.post(
    "/auth/revoke-refresh-token",
    validateRequest("/auth/revoke-refresh-token"),
    authController.handleRevokeRefreshToken
);

authRoute.post(
    "/auth/confirm-email",
    validateRequest("/auth/confirm-email"),
    authController.handleConfirmEmail
);
authRoute.post(
    "/auth/email-confirmation-link",
    validateRequest("/auth/email-confirmation-link"),
    authController.handleSendEmailConfirmationLink
);

authRoute.post(
    "/auth/reset-password",
    validateRequest("/auth/reset-password"),
    authController.handleResetPassword
);
authRoute.post(
    "/auth/password-reset-link",
    validateRequest("/auth/password-reset-link"),
    authController.handleSendPasswordResetLink
);

authRoute.post(
    "/auth/google-login",
    validateRequest("/auth/google-login"),
    authController.handleGoogleLogin
);

export default authRoute;