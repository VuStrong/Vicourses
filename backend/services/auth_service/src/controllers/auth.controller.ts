import { NextFunction, Request, Response } from "express";
import * as signInService from "../services/signin.service";
import * as userSerivce from "../services/user.service";

export async function handleLogin(req: Request, res: Response, next: NextFunction) {
    const { email, password } = req.body;
    
    try {
        const { user, accessToken, refreshToken } = await signInService.signInWithEmailAndPassword(email, password);
        
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
        const newUser = await userSerivce.createUser({
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
        const newAccessToken = await signInService.refreshAccessToken(userId, refreshToken);

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
        await signInService.revokeRefreshToken(userId, refreshToken);

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
        await userSerivce.confirmEmail(userId, token);
        
        return res.send({ success: true });
    } catch (error) {
        next(error);
    }
}

export async function handleSendEmailConfirmationLink(req: Request, res: Response, next: NextFunction) {
    const { email } = req.body;

    try {
        await userSerivce.sendEmailConfirmationLink(email);
        
        res.send({ success: true });
    } catch (error) {
        next(error);        
    }
}

export async function handleResetPassword(req: Request, res: Response, next: NextFunction) {
    const { userId, token, newPassword } = req.body;

    try {
        await userSerivce.resetPassword(userId, token, newPassword);
        
        return res.send({ success: true });
    } catch (error) {
        next(error);
    }
}

export async function handleSendPasswordResetLink(req: Request, res: Response, next: NextFunction) {
    const { email } = req.body;

    try {
        await userSerivce.sendPasswordResetLink(email);
        
        res.send({ success: true });
    } catch (error) {
        next(error);
    }
}

export async function handleGoogleLogin(req: Request, res: Response, next: NextFunction) {
    const { idToken } = req.body;

    try {
        const { user, accessToken, refreshToken } = await signInService.signInWithGoogle(idToken);
        
        res.status(200).send({
            user,
            accessToken,
            refreshToken
        });
    } catch (error) {
        next(error);
    }
}