export interface OrderDetailsDto {
    id: number;
    buyerId: number;
    status: number;
    createdAt: Date;

    address: {
        country: string;
        city: string;
        street: string;
        zipCode: string;
    };

    orderItems: { productId: string; productImageUrl: string; productPrice: number; quantity: number }[];
}