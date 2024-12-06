export type PagedResult<T> = {
    skip: number;
    limit: number;
    total: number;
    end: boolean;
    items: T[];
}