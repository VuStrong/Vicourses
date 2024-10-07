import jwt from "jsonwebtoken";
import fs from "fs";

export function verify(token: string) {
    if (!fs.existsSync("public.key")) {
        throw new Error("Public key is missing!");
    }

    var publicKey = fs.readFileSync('public.key');

    return jwt.verify(token, publicKey);
}