import jwt from "jsonwebtoken";
import fs from "fs";
import User from "../entities/user.entity";
import Config from "../config";

export function signForUser(user: User) {
    if (!fs.existsSync("private.key")) {
        throw new Error("Private key is missing!");
    }

    var privateKey = fs.readFileSync("private.key");

    const token = jwt.sign(
        {
            sub: user.id,
            email: user.email,
            role: user.role,
            emailConfirmed: user.emailConfirmed,
        },
        privateKey,
        {
            expiresIn: "30m",
            algorithm: "RS256",
        }
    );

    return token;
}

export function verifyAccessToken(token: string) {
    if (!fs.existsSync("public.key")) {
        throw new Error("Public key is missing!");
    }

    var publicKey = fs.readFileSync("public.key");

    return jwt.verify(token, publicKey);
}

export function verifyFileUploadToken(token: string): {
    fileId: string,
    url: string,
    userId: string,
} {
    const payload = jwt.verify(token, Config.FileUploadSecret || "") as any;

    if (!payload.fileId || !payload.url || !payload.userId) {
        throw new Error("Invalid token");
    }

    return {
        fileId: payload.fileId,
        url: payload.url,
        userId: payload.userId
    };
}