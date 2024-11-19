import { NextFunction, Request, Response } from "express";
import * as userSerivce from "../services/user.service";
import { Role } from "../entity/user.entity";

export async function handleLockUser(req: Request, res: Response, next: NextFunction) {
    const userId = req.params.id;
    const days = +req.body.days;

    try {
        await userSerivce.setLockedOut(userId, days);
        
        res.status(200).send({
            success: true,
        });
    } catch (error) {
       next(error);
    }
}

export async function handleSetUserRole(req: Request, res: Response, next: NextFunction) {
    const userId = req.params.id;
    const role = req.body.role;

    try {
        await userSerivce.setRole(userId, role as Role);
        
        res.status(200).send({
            success: true,
        });
    } catch (error) {
       next(error);
    }
}

export async function handleGetUser(req: Request, res: Response, next: NextFunction) {
    const userId = req.params.id;

    try {
        const user = await userSerivce.getUserById(userId);
        
        res.status(200).send(user);
    } catch (error) {
       next(error);
    }
}