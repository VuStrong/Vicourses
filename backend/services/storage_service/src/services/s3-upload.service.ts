import {
    S3Client,
    PutObjectCommand,
    DeleteObjectCommand,
    CreateMultipartUploadCommand,
    UploadPartCommand,
    CompleteMultipartUploadCommand,
    CompletedPart,
    AbortMultipartUploadCommand,
} from "@aws-sdk/client-s3";
import { getSignedUrl } from "@aws-sdk/s3-request-presigner";
import { randomUUID } from "crypto";
import path from "path";
import Config from "../config";
import { AppError } from "../utils/app-error";

const S3 = new S3Client({
    region: "auto",
    endpoint: `${Config.S3_ENDPOINT}`,
    credentials: {
        accessKeyId: `${Config.S3_ACCESS_KEY_ID}`,
        secretAccessKey: `${Config.S3_ACCESS_KEY_SECRET}`,
    },
});

type UploadOptions = {
    fileId?: string;
};

type UploadResponse = {
    url: string;
    fileId: string;
};

type initializeMultipartUploadResponse = {
    uploadId?: string;
    parts: {
        url: string;
        partNumber: number;
    }[];
};

export async function uploadSingleFile(
    file: Express.Multer.File,
    options?: UploadOptions
): Promise<UploadResponse> {
    let { fileId } = options ?? {};
    const ext = path.extname(file.originalname);
    fileId = fileId || `${randomUUID()}${ext}`;

    const cmd = new PutObjectCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        Body: file.buffer,
        ContentType: file.mimetype,
    });

    await S3.send(cmd);

    return {
        url: `${Config.S3_DOMAIN}/${fileId}`,
        fileId,
    };
}

export async function deleteSingleFile(fileId: string): Promise<void> {
    const cmd = new DeleteObjectCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
    });

    await S3.send(cmd);
}

export async function initializeMultipartUpload(
    fileId?: string,
    partCount: number = 1
): Promise<initializeMultipartUploadResponse> {
    if (partCount <= 0 || partCount >= 1000)
        throw new AppError("partCount must be between 1 and 1000", 400);

    if (!fileId) fileId = `${randomUUID()}`;

    const cmd = new CreateMultipartUploadCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        ACL: "public-read",
    });
    const { UploadId } = await S3.send(cmd);

    const tasks = [];
    for (let index = 0; index < partCount; index++) {
        tasks.push(
            getSignedUrl(
                S3,
                new UploadPartCommand({
                    Bucket: Config.S3_BUCKET_NAME,
                    Key: fileId,
                    UploadId,
                    PartNumber: index + 1,
                }),
                { expiresIn: 36000 }
            )
        );
    }

    const signedUrls = await Promise.all(tasks);

    return {
        uploadId: UploadId,
        parts: signedUrls.map((url, index) => ({
            url,
            partNumber: index + 1,
        })),
    };
}

export async function completeMultipartUpload(
    uploadId: string,
    fileId: string,
    parts: CompletedPart[]
) {
    const cmd = new CompleteMultipartUploadCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        UploadId: uploadId,
        MultipartUpload: {
            Parts: parts.sort((a, b) => a.PartNumber! - b.PartNumber!),
        },
    });

    await S3.send(cmd);
}

export async function abortMultipartUpload(uploadId: string, fileId: string) {
    const cmd = new AbortMultipartUploadCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        UploadId: uploadId,
    });

    await S3.send(cmd);
}
