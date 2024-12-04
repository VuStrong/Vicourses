import { v4 as uuidv4 } from "uuid";
import { getFileExtension } from "./utils";
import {
    abortMultipartUpload,
    completeMultipartUpload,
    initializeMultipartUpload,
} from "@/services/api/storage";
import { CompletePartRequest, PartResponse } from "./types/storage";

interface S3ChunkUploaderOptions {
    chunkSize?: number;
    file: File;
    fileId?: string;
    accessToken: string;
    onError?: (error: Error) => void;
    onProgress?: (percentage: number) => void;
    onComplete?: (fileId: string) => void;
}

export default class S3ChunkUploader {
    private readonly chunkSize: number;
    private readonly file: File;
    private readonly fileId: string;
    public accessToken: string;

    private uploadId?: string;
    private aborted = false;
    private done = false;
    private uploadedSize = 0;
    private progressCache = 0;
    private activeRequest?: XMLHttpRequest;
    private parts: PartResponse[] = [];
    private uploadedParts: CompletePartRequest[] = [];

    private onProgressCb?: (percentage: number) => void;
    private onErrorCb?: (error: Error) => void;
    private onCompleteCb?: (fileId: string) => void;

    constructor(options: S3ChunkUploaderOptions) {
        this.chunkSize = options.chunkSize || 1024 * 1024 * 5;

        if (this.chunkSize < 1024 * 1024 * 5) {
            throw new Error("chunkSize must larger than 5MB");
        }

        this.file = options.file;
        this.fileId =
            options.fileId || `${uuidv4()}.${getFileExtension(this.file)}`;
        this.accessToken = options.accessToken;

        this.onErrorCb = options.onError;
        this.onProgressCb = options.onProgress;
        this.onCompleteCb = options.onComplete;
    }

    start() {
        if (this.aborted) {
            throw new Error("This uploader already aborted");
        }
        if (this.done) {
            return;
        }

        (async () => {
            try {
                const partCount = Math.ceil(this.file.size / this.chunkSize);

                const result = await initializeMultipartUpload(
                    this.fileId,
                    partCount,
                    this.accessToken
                );

                this.uploadId = result.uploadId;
                this.parts.push(...result.parts);

                await this.uploadParts();
            } catch (error: any) {
                this.onErrorCb?.(error);
                this.abort();
            }
        })();
    }

    private async uploadParts() {
        const length = this.parts.length;

        if (length <= 0) return;

        let allSuccess = true;

        for (let index = 0; index < length; index++) {
            const part = this.parts[index];
            const sentSize = (part.partNumber - 1) * this.chunkSize;
            const chunk = this.file.slice(sentSize, sentSize + this.chunkSize);

            try {
                const status = await this.uploadChunk(chunk, part);

                if (status === "aborted") {
                    return;
                }
            } catch (error: any) {
                this.onErrorCb?.(error);
                allSuccess = false;
                break;
            }
        }

        if (allSuccess) {
            try {
                await this.sendCompleteRequest();
                this.done = true;
                this.onCompleteCb?.(this.fileId);
            } catch (error: any) {
                this.onErrorCb?.(error);
                this.abort();
            }
        } else {
            this.abort();
        }
    }

    private async uploadChunk(
        chunk: Blob,
        part: PartResponse
    ): Promise<"success" | "aborted"> {
        return new Promise<"success" | "aborted">((resolve, reject) => {
            const xhr = (this.activeRequest = new XMLHttpRequest());

            const progressListener = this.handleProgress.bind(this);

            xhr.upload.addEventListener("progress", progressListener);

            xhr.addEventListener("error", progressListener);
            xhr.addEventListener("abort", progressListener);
            xhr.addEventListener("loadend", progressListener);

            xhr.open("PUT", part.url);

            xhr.onreadystatechange = () => {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    const ETag = xhr.getResponseHeader("ETag");

                    if (ETag) {
                        const uploadedPart = {
                            PartNumber: part.partNumber,
                            ETag: ETag.replaceAll('"', ""),
                        };

                        this.uploadedParts.push(uploadedPart);

                        resolve("success");
                    } else {
                        reject(new Error("Cannot get ETag"));
                    }
                }
            };

            xhr.onerror = (error) => {
                reject(error);
            };

            xhr.onabort = () => {
                resolve("aborted");
            };

            xhr.send(chunk);
        });
    }

    private async sendCompleteRequest() {
        if (this.fileId && this.uploadId) {
            await completeMultipartUpload(
                {
                    fileId: this.fileId,
                    uploadId: this.uploadId,
                    parts: this.uploadedParts,
                },
                this.accessToken
            );
        }
    }

    private handleProgress(event: any) {
        if (this.file) {
            if (
                event.type === "progress" ||
                event.type === "error" ||
                event.type === "abort"
            ) {
                this.progressCache = event.loaded;
            }

            if (event.type === "loadend") {
                this.uploadedSize += this.progressCache;
                this.progressCache = 0;
            }

            const sentSize = Math.min(
                this.uploadedSize + this.progressCache,
                this.file.size
            );

            const totalSize = this.file.size;

            const percentage = Math.round((sentSize / totalSize) * 100);

            this.onProgressCb?.(percentage);
        }
    }

    abort() {
        if (this.done || this.aborted) return;

        this.activeRequest?.abort();

        this.aborted = true;

        if (this.uploadId && this.fileId) {
            abortMultipartUpload(this.uploadId, this.fileId, this.accessToken);
        }

        this.uploadId = undefined;
    }
}
