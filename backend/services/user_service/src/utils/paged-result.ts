export default class PagedResult<T> {
    skip: number;
    limit: number;
    total: number;
    end: boolean;
    items: T[];

    constructor(items: T[], skip: number, limit: number, total: number) {
        this.skip = skip;
        this.limit = limit;
        this.total = total;
        this.end = limit + skip >= total;
        this.items = items;        
    }
}