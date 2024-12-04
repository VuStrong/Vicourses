import { BACKEND_URL } from "@/libs/constants";
import { CompleteMultipartUploadRequest, InitializeMultipartUploadResponse } from "@/libs/types/storage";

export async function initializeMultipartUpload(
    fileId: string,
    partCount: number,
    accessToken: string
): Promise<InitializeMultipartUploadResponse> {
    const res = await fetch(
        `${BACKEND_URL}/api/sts/v1/initialize-multipart-upload`,
        {
            method: "POST",
            body: JSON.stringify({ fileId, partCount }),
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

export async function completeMultipartUpload(request: CompleteMultipartUploadRequest, accessToken: string) {
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