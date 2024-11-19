import { NextFunction, Request, Response } from "express";
import { verifyJwt } from "../utils/jwt";

export default function authenticate(req: Request, res: Response, next: NextFunction) {
    const token = getTokenFromHeader(req);
    
    if (!token) {
        return res.status(401).send({
            message: "Unauthorized!",
            code: 401,
        });
    }

    try {
        const payload = verifyJwt(token);

        req.user = payload as any;
    } catch (error) {
        return res.status(401).send({
            message: "Unauthorized!",
            code: 401,
        });
    }

    next();
}

function getTokenFromHeader(req: Request): string | undefined {
    const [type, token] = req.headers.authorization?.split(" ") ?? [];
    return type === "Bearer" ? token : undefined;
}