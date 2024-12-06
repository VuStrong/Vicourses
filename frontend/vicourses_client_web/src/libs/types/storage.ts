export type UploadResponse = {
    token: string;
    url: string;
    fileId: string;
    originalFileName: string;
}

export type InitializeMultipartUploadResponse = {
    uploadId: string;
    parts: PartResponse[];
}

export type PartResponse = {
    url: string;
    partNumber: number;
}

export type CompleteMultipartUploadRequest = {
    fileId: string;
    uploadId: string;
    parts: CompletePartRequest[];
}

export type CompletePartRequest = {
    PartNumber: number;
    ETag: string;
}
