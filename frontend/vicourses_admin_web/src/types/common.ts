export type VideoStatus = "BeingProcessed" | "Processed" | "ProcessingFailed";

export type PagedResult<T> = {
    skip: number;
    limit: number;
    total: number;
    end: boolean;
    items: T[];
}