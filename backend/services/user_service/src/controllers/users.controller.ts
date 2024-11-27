import { Request, Response, NextFunction } from "express";
import { getUserById, getUsers, setLockedOut } from "../services/users.service";
import User, { Role } from "../entities/user.entity";

export async function handleGetListOfUsers(req: Request, res: Response, next: NextFunction) {
    const fields = req.query.fields as string;
    const skip = Number(req.query.skip);
    const limit = Number(req.query.limit);
    const role = req.query.role as Role;
    const order = (req.query.order as string)?.split("_");

    try {
        const result = await getUsers({
            skip,
            limit,
            role,
            order: order
                ? {
                      [order[0]]: order[1],
                  }
                : undefined,
            fields: fields?.split(",") as (keyof User)[],
        });

        res.status(200).send(result);
    } catch (error) {
        next(error);
    }
}

export async function handleGetUser(req: Request, res: Response, next: NextFunction) {
    const userId = req.params.id;
    const fields = req.query.fields as string;

    try {
        const user = await getUserById(
            userId,
            fields?.split(",") as (keyof User)[]
        );

        res.status(200).send(user);
    } catch (error) {
        next(error);
    }
}

export async function handleGetPublicProfile(req: Request, res: Response, next: NextFunction) {
    const authenticatedUserId = req.user?.sub;
    const authenticatedUserRole = req.user?.role;
    const userId = req.params.id;

    try {
        const user = await getUserById(userId, [
            "id",
            "createdAt",
            "name",
            "role",
            "thumbnailUrl",
            "headline",
            "description",
            "websiteUrl",
            "youtubeUrl",
            "facebookUrl",
            "linkedInUrl",
            "totalEnrollmentCount",
            "isPublic",
        ]);

        if (
            !user.isPublic &&
            user.id !== authenticatedUserId &&
            authenticatedUserRole !== "admin"
        ) {
            return res.status(403).send({
                message: "You do not have permission to view this profile",
                code: 403,
            });
        }

        res.status(200).send(user);
    } catch (error) {
        next(error);
    }
}

export async function handleLockUser(req: Request, res: Response, next: NextFunction) {
    const userId = req.params.id;
    const days = +req.body.days;

    try {
        await setLockedOut(userId, days);

        res.status(200).send({ success: true });
    } catch (error) {
        next(error);
    }
}
