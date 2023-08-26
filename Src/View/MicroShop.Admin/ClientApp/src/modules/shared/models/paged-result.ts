export interface Result {
    status: number;
    statusText: string;
}

export interface DataResult<T> extends Result {
    data: T;
}

export interface PagedResult<T> extends DataResult<T> {
    pager: {
        totalCount: number;
        totalPages: number;
        currentPage: number;
    }
}