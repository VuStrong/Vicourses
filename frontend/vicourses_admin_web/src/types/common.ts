export type VideoStatus = "BeingProcessed" | "Processed" | "ProcessingFailed";

export type VideoFile = {
    originalFileName: string;
    duration: number;
    status: VideoStatus;
    token: string | null;
}

export type PagedResult<T> = {
    skip: number;
    limit: number;
    total: number;
    end: boolean;
    items: T[];
}