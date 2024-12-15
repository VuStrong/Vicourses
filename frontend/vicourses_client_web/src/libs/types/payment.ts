export type PaymentStatus = "Pending" | "Completed" | "Refunded";
export type PaymentMethod = "Paypal";

export type Payment = {
    id: string;
    userId: string;
    username: string;
    email: string;
    courseId: string;
    courseName: string;
    createdAt: string;
    paymentDueDate: string;
    refundDueDate: string;
    listPrice: number;
    discount: number;
    totalPrice: number;
    couponCode: string | null;
    status: PaymentStatus;
    method: PaymentMethod;
}

export type GetUserPaymentsQuery = {
    skip?: number;
    limit?: number;
    status?: PaymentStatus;
}

export type CreatePaypalPaymentRequest = {
    courseId: string;
    couponCode?: string;
}