import { NextFunction, Request, Response } from "express";
import { getHlsManifestUrl } from "../services/media.service";
import { AppError } from "../utils/app-error";

export async function handleGetHlsManifestUrl(
    req: Request,
    res: Response,
    next: NextFunction
) {
    if (!req.query.token) {
        return next(new AppError("Media token is missing", 400));
    }

    const token = `${req.query.token}`;

    try {
        const result = await getHlsManifestUrl(token);

        res.status(200).send(result);
    } catch (error) {
        next(error);
    }
}
