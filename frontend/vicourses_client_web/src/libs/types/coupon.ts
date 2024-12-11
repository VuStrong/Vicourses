export type Coupon = {
    id: string;
    code: string;
    creatorId: string;
    courseId: string;
    createdAt: string;
    expiryDate: string;
    count: number;
    remain: number;
    discount: number;
    isActive: boolean;
}

export type CreateCouponRequest = {
    courseId: string;
    days: number;
    availability: number;
    discount: number;
}

export type GetCouponsQuery = {
    skip?: number;
    limit?: number;
    courseId: string;
    isExpired?: boolean;
}

export type CheckCouponResponse = {
    code: string;
    discount: number;
    price: number;
    discountPrice: number;
    remain: number;
    isValid: boolean;
    details: string | null;
}