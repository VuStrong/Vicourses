import { NextFunction, Request, Response } from "express";
import jwt from "jsonwebtoken";
import * as s3uploadService from "../services/s3-upload.service";
import { AppError } from "../utils/app-error";
import Config from "../config";

export async function handleUploadImage(req: Request, res: Response, next: NextFunction) {
    if (!req.file) {
        return next(new AppError("Image file is required", 400));
    }

    try {
        const result = await s3uploadService.uploadSingleFile({
            file: req.file,
            fileId: req.body.fileId,
        });

        const token = jwt.sign({
            ...result,
            userId: req.user.sub,
        }, Config.FILE_UPLOAD_SECRET || "", { expiresIn: "10m" });
        
        res.status(201).send({ token });
    } catch (error) {
        next(error);
    }
}

export async function handleInitializeS3MultipartUpload(req: Request, res: Response, next: NextFunction) {
    const fileId = req.body.fileId;
    const fileName = req.body.fileName;
    const partCount = +req.body.partCount;

    try {
        const result = await s3uploadService.initializeMultipartUpload({
            fileId,
            partCount,
            fileName,
        });
        
        res.status(201).send(result);
    } catch (error) {
        next(error);
    }
}

export async function handleCompleteS3MultipartUpload(req: Request, res: Response, next: NextFunction) {
    const { fileId, uploadId, parts } = req.body;

    try {
        const result = await s3uploadService.completeMultipartUpload(uploadId, fileId, parts);

        const token = jwt.sign({
            ...result,
            userId: req.user.sub,
        }, Config.FILE_UPLOAD_SECRET || "", { expiresIn: "10m" });
        
        res.status(200).send({ token });
    } catch (error) {
        next(error);
    }
}

export async function handleAbortS3MultipartUpload(req: Request, res: Response, next: NextFunction) {
    const { fileId, uploadId } = req.body;
    
    try {
        await s3uploadService.abortMultipartUpload(uploadId, fileId);

        res.status(200).send({ message: "Success" });
    } catch (error) {
        next(error);
    }
}