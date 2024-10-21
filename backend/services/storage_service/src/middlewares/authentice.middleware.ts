import { NextFunction, Request, Response } from "express";
import jwt from "jsonwebtoken";
import fs from "fs";

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

function verifyJwt(token: string) {
    if (!fs.existsSync("public.key")) {
        throw new Error("Public key is missing!");
    }

    var publicKey = fs.readFileSync('public.key');

    return jwt.verify(token, publicKey);
}