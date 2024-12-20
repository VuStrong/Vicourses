import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import {
    CreatePaypalPaymentRequest,
    GetUserPaymentsQuery,
    Payment,
} from "@/libs/types/payment";

export async function getUserPayments(
    query: GetUserPaymentsQuery,
    accessToken: string
): Promise<PagedResult<Payment> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(`${BACKEND_URL}/api/ps/v1/payments/my-payments?${params}`, {
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

export async function getPayment(
    paymentId: string,
    accessToken: string
): Promise<Payment | null> {
    const res = await fetch(`${BACKEND_URL}/api/ps/v1/payments/${paymentId}`, {
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

export async function createPaypalPayment(
    request: CreatePaypalPaymentRequest,
    accessToken: string
): Promise<Payment> {
    const res = await fetch(`${BACKEND_URL}/api/ps/v1/payments/paypal`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function capturePaypalPayment(
    paypalOrderId: string,
    accessToken: string
) {
    const res = await fetch(
        `${BACKEND_URL}/api/ps/v1/payments/paypal/${paypalOrderId}/capture`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function cancelPayment(paymentId: string, accessToken: string) {
    const res = await fetch(
        `${BACKEND_URL}/api/ps/v1/payments/${paymentId}/cancel`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function refundPayment(
    paymentId: string,
    accessToken: string,
    reason?: string
) {
    let url = `${BACKEND_URL}/api/ps/v1/payments/${paymentId}/refund`;

    if (reason) {
        url += `?reason=${reason}`;
    }

    const res = await fetch(url, {
        method: "POST",
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
