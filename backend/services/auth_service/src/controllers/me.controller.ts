import { NextFunction, Request, Response } from "express";
import * as userSerivce from "../services/user.service";

export async function handleChangePassword(req: Request, res: Response, next: NextFunction) {
    const { oldPassword, newPassword } = req.body;

    try {
        await userSerivce.changePassword(req.user.sub, oldPassword, newPassword);
        
        res.status(200).send({
            success: true,
        });
    } catch (error) {
       next(error);
    }
}

export async function handleGetUser(req: Request, res: Response, next: NextFunction) {
    try {
        const user = await userSerivce.getUserById(req.user.sub);
        
        res.status(200).send(user);
    } catch (error) {
       next(error);
    }
}