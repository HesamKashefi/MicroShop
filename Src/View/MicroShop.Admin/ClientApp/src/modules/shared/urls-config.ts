export class UrlsConfig {
    static catalog_getCatalog(base: string, page: number = 1) {
        return `${base}products?page=${page}`;
    }
    static catalog_getProductById(base: string, id: string) {
        return `${base}products/${id}`;
    }
    static catalog_updateProductPrice(base: string) {
        return `${base}products/price`;
    }
    static catalog_updateProductInfo(base: string) {
        return `${base}products/info`;
    }


    static orders_getAll(base: string, page: number = 1) {
        return `${base}orders/all?page=${page}`;
    }
    static orders_getById(base: string, orderId: number) {
        return `${base}orders/${orderId}`;
    }
}