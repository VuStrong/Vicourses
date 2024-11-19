import jwt from "jsonwebtoken";
import fs from "fs";
import User from "../entity/user.entity";
import Config from "../config";

export async function signForUser(user: User) {
    if (!fs.existsSync("private.key")) {
        throw new Error("Private key is missing!");
    }

    var privateKey = fs.readFileSync('private.key');

    const token = jwt.sign({
        sub: user.id,
        email: user.email,
        role: user.role,
        emailConfirmed: user.emailConfirmed,
    }, privateKey, { 
        expiresIn: Config.JWT_LIFETIME,
        algorithm: "RS256"
    });

    return token;
}

export function verifyJwt(token: string) {
    if (!fs.existsSync("public.key")) {
        throw new Error("Public key is missing!");
    }

    var publicKey = fs.readFileSync('public.key');

    return jwt.verify(token, publicKey);
}