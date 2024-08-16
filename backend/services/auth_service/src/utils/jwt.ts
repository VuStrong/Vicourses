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
        locked: user.locked,
    }, privateKey, { 
        expiresIn: Config.JWT_LIFETIME,
        algorithm: "RS256"
    });

    return token;
}
