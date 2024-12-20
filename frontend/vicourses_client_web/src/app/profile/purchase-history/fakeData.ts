import { Payment } from "@/libs/types/payment";

export const paymentsFake: Payment[] = [
    {
        id: "payment1",
        userId: "user1",
        username: "Vu Manh",
        email: "vumanh@gmail.com",
        courseId: "course1",
        courseName: "From ReactJS to Jobless - ULTIMATE course",
        createdAt: "2024-12-15T08:29:36.603Z",
        paymentDueDate: "2024-12-15T08:29:36.603Z",
        refundDueDate: "2024-12-18T08:29:36.603Z",
        listPrice: 29.99,
        discount: 50,
        totalPrice: 14.99,
        couponCode: null,
        status: "Pending",
        method: "Paypal",
        paypalOrderId: null,
    },
    {
        id: "payment2",
        userId: "user1",
        username: "Vu Manh",
        email: "vumanh@gmail.com",
        courseId: "course2",
        courseName: "From ReactJS to ABCDEF - ULTIMATE course",
        createdAt: "2024-12-15T08:29:36.603Z",
        paymentDueDate: "2024-12-15T08:29:36.603Z",
        refundDueDate: "2024-12-18T08:29:36.603Z",
        listPrice: 29.99,
        discount: 50,
        totalPrice: 14.99,
        couponCode: null,
        status: "Completed",
        method: "Paypal",
        paypalOrderId: null,
    },
    {
        id: "payment3",
        userId: "user1",
        username: "Vu Manh",
        email: "vumanh@gmail.com",
        courseId: "course3",
        courseName: "From ReactJS to AAAAAAA - ULTIMATE course",
        createdAt: "2024-12-15T08:29:36.603Z",
        paymentDueDate: "2024-12-15T08:29:36.603Z",
        refundDueDate: "2024-12-18T08:29:36.603Z",
        listPrice: 19.99,
        discount: 50,
        totalPrice: 9.99,
        couponCode: null,
        status: "Refunded",
        method: "Paypal",
        paypalOrderId: null,
    },
];