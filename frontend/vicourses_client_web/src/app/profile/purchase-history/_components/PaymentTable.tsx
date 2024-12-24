"use client";

import { useState } from "react";
import { Button, Card, Chip, Typography } from "@material-tailwind/react";

import { Payment } from "@/libs/types/payment";
import RefundModal from "./RefundModal";

export default function PaymentTable({
    payments,
    end,
    next,
    onPaymentRefunded,
}: {
    payments: Payment[];
    end: boolean;
    next: () => Promise<void>;
    onPaymentRefunded: (paymentId: string) => void;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [openRefundModal, setOpenRefundModal] = useState<boolean>(false);
    const [payment, setPayment] = useState<Payment>();
    const now = new Date();

    const handleClickRefund = (payment: Payment) => {
        setPayment(payment);
        setOpenRefundModal(true);
    };

    return (
        <>
            <RefundModal
                open={openRefundModal}
                close={() => setOpenRefundModal(false)}
                payment={payment}
                onPaymentRefunded={onPaymentRefunded}
            />
            <Card className="h-full w-full overflow-x-scroll">
                <table className="w-full min-w-max table-auto text-left">
                    <thead>
                        <tr>
                            <th className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4">
                                <Typography
                                    variant="small"
                                    color="blue-gray"
                                    className="font-normal leading-none opacity-70"
                                >
                                    Course
                                </Typography>
                            </th>
                            <th className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4">
                                <Typography
                                    variant="small"
                                    color="blue-gray"
                                    className="font-normal leading-none opacity-70"
                                >
                                    Date
                                </Typography>
                            </th>
                            <th className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4">
                                <Typography
                                    variant="small"
                                    color="blue-gray"
                                    className="font-normal leading-none opacity-70"
                                >
                                    Total
                                </Typography>
                            </th>
                            <th className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4">
                                <Typography
                                    variant="small"
                                    color="blue-gray"
                                    className="font-normal leading-none opacity-70"
                                >
                                    Status
                                </Typography>
                            </th>
                            <th className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4">
                                <Typography
                                    variant="small"
                                    color="blue-gray"
                                    className="font-normal leading-none opacity-70"
                                >
                                    Method
                                </Typography>
                            </th>
                            <th className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {payments.map((payment, index) => {
                            const isLast = index === payments.length - 1;
                            const classes = isLast
                                ? "p-4"
                                : "p-4 border-b border-blue-gray-50";

                            return (
                                <tr key={payment.id}>
                                    <td className={classes}>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-bold"
                                        >
                                            {payment.courseName}
                                        </Typography>
                                    </td>
                                    <td className={classes}>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal"
                                        >
                                            {new Date(
                                                payment.createdAt
                                            ).toLocaleDateString()}
                                        </Typography>
                                    </td>
                                    <td className={classes}>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal"
                                        >
                                            ${payment.totalPrice}
                                        </Typography>
                                    </td>
                                    <td className={classes}>
                                        <div className="w-max">
                                            <Chip
                                                size="sm"
                                                variant="ghost"
                                                value={payment.status}
                                                color={
                                                    payment.status ===
                                                    "Completed"
                                                        ? "green"
                                                        : payment.status ===
                                                          "Pending"
                                                        ? "amber"
                                                        : "red"
                                                }
                                            />
                                        </div>
                                    </td>
                                    <td className={classes}>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal"
                                        >
                                            {payment.method}
                                        </Typography>
                                    </td>
                                    {payment.status === "Completed" &&
                                        new Date(payment.refundDueDate) >
                                            now && (
                                            <td className={classes}>
                                                <Button
                                                    variant="text"
                                                    className="text-primary"
                                                    size="sm"
                                                    onClick={() =>
                                                        handleClickRefund(
                                                            payment
                                                        )
                                                    }
                                                >
                                                    Refund
                                                </Button>
                                            </td>
                                        )}
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </Card>
            {!end && (
                <div className="flex items-center justify-center mt-5">
                    <Button
                        loading={isLoading}
                        className="bg-transparent text-gray-900 border border-gray-900"
                        onClick={async () => {
                            setIsLoading(true);
                            await next();
                            setIsLoading(false);
                        }}
                    >
                        Load more
                    </Button>
                </div>
            )}
        </>
    );
}
