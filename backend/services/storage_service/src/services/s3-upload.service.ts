import {
    S3Client,
    PutObjectCommand,
    DeleteObjectCommand,
    CreateMultipartUploadCommand,
    UploadPartCommand,
    CompleteMultipartUploadCommand,
    CompletedPart,
    AbortMultipartUploadCommand,
    NoSuchUpload,
    DeleteObjectsCommand,
    HeadObjectCommand,
    NotFound,
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

type UploadSingleFileParams = {
    file: Express.Multer.File;
    fileId?: string;
};

type InitializeMultipartUploadParams = {
    fileId?: string,
    partCount?: number;
    fileName?: string;
};

type UploadResponse = {
    url: string;
    fileId: string;
    originalFileName?: string;
};

type InitializeMultipartUploadResponse = {
    uploadId?: string;
    fileId: string;
    parts: {
        url: string;
        partNumber: number;
    }[];
};

export async function uploadSingleFile(
    params: UploadSingleFileParams
): Promise<UploadResponse> {
    let { file, fileId } = params;
    const ext = path.extname(file.originalname);

    fileId = fileId?.trim();
    if (!fileId) fileId = `${randomUUID()}${ext}`;

    if (await checkFileExists(fileId)) {
        throw new AppError("fileId already exists", 403);
    }

    const cmd = new PutObjectCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        Body: file.buffer,
        ContentType: file.mimetype,
        Metadata: {
            'original-name': file.originalname
        }
    });

    await S3.send(cmd);

    return {
        url: `${Config.S3_DOMAIN}/${fileId}`,
        fileId,
        originalFileName: file.originalname,
    };
}

export async function checkFileExists(fileId: string): Promise<boolean> {
    const cmd = new HeadObjectCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
    });

    try {
        await S3.send(cmd);
    } catch (error) {
        if (error instanceof NotFound) {
            return false;
        }

        throw error;
    }

    return true;
}

export async function getFileMetadata(fileId: string): Promise<Record<string, string> | undefined> {
    const cmd = new HeadObjectCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
    });

    try {
        const result = await S3.send(cmd);

        return result.Metadata;
    } catch (error) {
        if (error instanceof NotFound) {
            throw new AppError("File not found", 404);
        }

        throw error;
    }
}

export async function deleteSingleFile(fileId: string): Promise<void> {
    const cmd = new DeleteObjectCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
    });

    await S3.send(cmd);
}

export async function deleteMultipleFiles(fileIds: string[]): Promise<void> {
    if (fileIds.length === 0) return;

    const cmd = new DeleteObjectsCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Delete: {
            Objects: fileIds.map((id) => ({ Key: id })),
        },
    });

    await S3.send(cmd);
}

export async function initializeMultipartUpload(params: InitializeMultipartUploadParams): Promise<InitializeMultipartUploadResponse> {
    let fileId = params.fileId;
    const partCount = params.partCount || 1;
    const fileName = params.fileName;

    if (partCount <= 0 || partCount >= 1000)
        throw new AppError("partCount must be between 1 and 999", 400);

    fileId = fileId?.trim();
    if (!fileId) fileId = `${randomUUID()}`;

    if (await checkFileExists(fileId)) {
        throw new AppError("fileId already exists", 403);
    }

    const cmd = new CreateMultipartUploadCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        ACL: "public-read",
        Metadata: {
            'original-name': fileName || ""
        }
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
        fileId,
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
): Promise<UploadResponse> {
    if (parts.length === 0) throw new AppError("parts must not empty", 400);

    const cmd = new CompleteMultipartUploadCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        UploadId: uploadId,
        MultipartUpload: {
            Parts: parts.sort((a, b) => a.PartNumber! - b.PartNumber!),
        },
    });

    try {
        await S3.send(cmd);

        const metadata = await getFileMetadata(fileId);

        return {
            fileId,
            url: `${Config.S3_DOMAIN}/${fileId}`,
            originalFileName: metadata?.["original-name"]
        };
    } catch (error) {
        if (error instanceof NoSuchUpload) {
            throw new AppError(error.message, 404);
        }

        throw error;
    }
}

export async function abortMultipartUpload(uploadId: string, fileId: string) {
    const cmd = new AbortMultipartUploadCommand({
        Bucket: Config.S3_BUCKET_NAME,
        Key: fileId,
        UploadId: uploadId,
    });

    try {
        await S3.send(cmd);
    } catch (error) {
        if (error instanceof NoSuchUpload) {
            throw new AppError(error.message, 404);
        }

        throw error;
    }
}
