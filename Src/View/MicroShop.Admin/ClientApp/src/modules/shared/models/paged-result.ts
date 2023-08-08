export interface PagedResult<T> {
    data: T;
    totalCount: number;
    totalPages: number;
    currentPage: number;
}