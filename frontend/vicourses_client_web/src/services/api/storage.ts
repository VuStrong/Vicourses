import { BACKEND_URL } from "@/libs/constants";
import {
    CompleteMultipartUploadRequest,
    InitializeMultipartUploadResponse,
    UploadResponse,
} from "@/libs/types/storage";

export async function uploadImage(
    file: File,
    accessToken: string,
    fileId?: string
): Promise<UploadResponse> {
    const formData = new FormData();
    formData.append("image", file);
    if (fileId) formData.append("fileId", fileId);

    const res = await fetch(`${BACKEND_URL}/api/sts/v1/upload-image`, {
        method: "POST",
        body: formData,
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function initializeMultipartUpload(
    request: {
        fileId: string,
        partCount: number,
        fileName?: string,
    },
    accessToken: string
): Promise<InitializeMultipartUploadResponse> {
    const res = await fetch(
        `${BACKEND_URL}/api/sts/v1/initialize-multipart-upload`,
        {
            method: "POST",
            body: JSON.stringify(request),
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data as InitializeMultipartUploadResponse;
}

export async function completeMultipartUpload(
    request: CompleteMultipartUploadRequest,
    accessToken: string
): Promise<UploadResponse> {
    const res = await fetch(
        `${BACKEND_URL}/api/sts/v1/complete-multipart-upload`,
        {
            method: "POST",
            body: JSON.stringify(request),
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function abortMultipartUpload(
    uploadId: string,
    fileId: string,
    accessToken: string
) {
    const res = await fetch(
        `${BACKEND_URL}/api/sts/v1/abort-multipart-upload`,
        {
            method: "POST",
            body: JSON.stringify({ uploadId, fileId }),
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}
