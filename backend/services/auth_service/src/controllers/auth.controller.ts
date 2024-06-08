import { NextFunction, Request, Response } from "express";
import * as authService from "../services";

export async function handleLogin(req: Request, res: Response, next: NextFunction) {
    const { email, password } = req.body;
    
    try {
        const { user, accessToken, refreshToken } = await authService.login(email, password);
        
        res.status(200).send({
            user,
            accessToken,
            refreshToken
        });
    } catch (error) {
       next(error);
    }
}

export async function handleRegister(req: Request, res: Response, next: NextFunction) {
    const { name, email, password } = req.body;
    
    try {
        const newUser = await authService.register({
            name, email, password
        });
    
        res.status(201).send({
            user: newUser
        });   
    } catch (error) {
        next(error);
    }
}

export async function handleRefreshToken(req: Request, res: Response, next: NextFunction) {
    const { refreshToken, userId } = req.body;

    try {
        const newAccessToken = await authService.refreshToken(userId, refreshToken);

        res.status(200).send({
            accessToken: newAccessToken,
            refreshToken
        });   
    } catch (error) {
        next(error);
    }
}

export async function handleRevokeRefreshToken(req: Request, res: Response, next: NextFunction) {
    const { refreshToken, userId } = req.body;

    try {
        await authService.revokeRefreshToken(userId, refreshToken);

        res.status(200).send({
            success: true
        });   
    } catch (error) {
        next(error);
    }
}

export async function handleConfirmEmail(req: Request, res: Response, next: NextFunction) {
    const { userId, token } = req.body;

    try {
        await authService.confirmEmail(userId, token);
        
        return res.send({ success: true });
    } catch (error) {
        next(error);
    }
}

export async function handleSendConfirmEmailLink(req: Request, res: Response, next: NextFunction) {
    const { email } = req.body;

    try {
        await authService.sendConfirmEmailLink(email);
        
        res.send({ success: true });
    } catch (error) {
        next(error);        
    }
}

export async function handleResetPassword(req: Request, res: Response, next: NextFunction) {
    const { userId, token, newPassword } = req.body;

    try {
        await authService.resetPassword(userId, token, newPassword);
        
        return res.send({ success: true });
    } catch (error) {
        next(error);
    }
}

export async function handleSendResetPasswordLink(req: Request, res: Response, next: NextFunction) {
    const { email } = req.body;

    try {
        await authService.sendResetPasswordLink(email);
        
        res.send({ success: true });
    } catch (error) {
        next(error);
    }
}

export async function handleGoogleLogin(req: Request, res: Response, next: NextFunction) {
    const { idToken } = req.body;

    try {
        const { user, accessToken, refreshToken } = await authService.googleLogin(idToken);
        
        res.status(200).send({
            user,
            accessToken,
            refreshToken
        });
    } catch (error) {
        next(error);
    }
}