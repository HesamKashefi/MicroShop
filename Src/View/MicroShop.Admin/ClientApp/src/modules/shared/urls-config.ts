export class UrlsConfig {
    static getCatalog(base: string, page: number = 1) {
        return `${base}products?page=${page}`;
    }
}