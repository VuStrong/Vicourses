import { NextFunction, Request, Response } from "express";
import { verifyAccessToken } from "../utils/jwt";

export default function authenticate(allowAnonymous = false) {
    return function (req: Request, res: Response, next: NextFunction) {
        const token = getTokenFromHeader(req);
        
        if (!token) {
            if (allowAnonymous) {
                return next();
            }

            return res.status(401).send({
                message: "Unauthorized!",
                code: 401,
            });
        }
    
        try {
            const payload = verifyAccessToken(token);
    
            req.user = payload as any;
        } catch (error) {
            return res.status(401).send({
                message: "Unauthorized!",
                code: 401,
            });
        }
    
        next();
    }
}

function getTokenFromHeader(req: Request): string | undefined {
    const [type, token] = req.headers.authorization?.split(" ") ?? [];
    return type === "Bearer" ? token : undefined;
}