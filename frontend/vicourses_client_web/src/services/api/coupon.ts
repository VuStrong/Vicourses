import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import {
    CheckCouponResponse,
    Coupon,
    CreateCouponRequest,
    GetCouponsQuery,
} from "@/libs/types/coupon";

export async function getCoupons(
    query: GetCouponsQuery,
    accessToken: string
): Promise<PagedResult<Coupon> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(`${BACKEND_URL}/api/ds/v1/coupons?${params}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function createCoupon(
    request: CreateCouponRequest,
    accessToken: string
): Promise<Coupon> {
    const res = await fetch(`${BACKEND_URL}/api/ds/v1/coupons`, {
        method: "POST",
        body: JSON.stringify(request),
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function updateCoupon(
    code: string,
    request: {
        isActive: boolean;
    },
    accessToken: string
) {
    const res = await fetch(`${BACKEND_URL}/api/ds/v1/coupons/${code}`, {
        method: "PATCH",
        body: JSON.stringify(request),
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function checkCoupon(
    code: string,
    courseId: string,
    accessToken: string
): Promise<CheckCouponResponse | null> {
    const res = await fetch(
        `${BACKEND_URL}/api/ds/v1/coupons/${code}/check?courseId=${courseId}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}
