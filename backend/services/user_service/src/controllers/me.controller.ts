import { Request, Response, NextFunction } from "express";
import { changePassword, getUserById, updateUserProfile } from "../services/users.service";
import User from "../entities/user.entity";

export async function handleGetAuthenticatedUser(req: Request, res: Response, next: NextFunction) {
    const userId = req.user!.sub;
    const fields = req.query.fields as string;

    try {
        const user = await getUserById(userId, fields?.split(",") as (keyof User)[]);

        res.status(200).send(user);
    } catch (error) {
        next(error);
    }
}

export async function handleUpdateUserProfile(req: Request, res: Response, next: NextFunction) {
    const userId = req.user!.sub;

    try {
        const user = await updateUserProfile(userId, req.body);

        res.status(200).send(user);
    } catch (error) {
        next(error);
    }
}

export async function handleChangePassword(req: Request, res: Response, next: NextFunction) {
    const userId = req.user!.sub;
    const { oldPassword, newPassword } = req.body;

    try {
        await changePassword(userId, oldPassword, newPassword);

        res.status(200).send({ success: true });
    } catch (error) {
        next(error);
    }
}