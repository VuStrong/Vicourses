import { NextFunction, Request, Response } from "express";

export default function checkRole(...roles: string[]) {
    return function (req: Request, res: Response, next: NextFunction) {
        if (roles.length === 0) {
            return next();
        }

        if (!req.user) {
            return res.status(401).send({
                message: "Unauthorized!",
                code: 401,
            });
        }

        var role = req.user.role;

        if (!roles.includes(role)) {
            return res.status(403).send({
                message: "Forbidden!",
                code: 403,
            });
        }

        next();
    }
}