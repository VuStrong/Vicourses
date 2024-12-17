"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { Select, Option } from "@material-tailwind/react";

import { Loader } from "@/components/common";
import { Payment, PaymentStatus } from "@/libs/types/payment";
import { getUserPayments } from "@/services/api/payment";
import PaymentTable from "./PaymentTable";
import { paymentsFake } from "./fakeData";

const limit = 15;

export default function PaymentsContainer() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [payments, setPayments] = useState<Payment[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);
    const [paymentStatus, setPaymentStatus] = useState<PaymentStatus>();
    const { data: session, status } = useSession();

    const getMorePayments = async () => {
        if (status === "authenticated") {
            const result = await getUserPayments(
                {
                    skip: skip + limit,
                    limit,
                    status: paymentStatus,
                },
                session.accessToken
            );

            if (result) {
                setPayments([...payments, ...result.items]);
                setEnd(result.end);
                setSkip(skip + limit);
                setIsLoading(false);
            }
        }
    };

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getUserPayments(
                    {
                        limit,
                        status: paymentStatus,
                    },
                    session.accessToken
                );

                if (result) {
                    setPayments(result.items);
                    setEnd(result.end);
                    setSkip(0);
                    setIsLoading(false);

                    // setPayments(paymentsFake);
                }
            })();
        }
    }, [status, paymentStatus]);

    return (
        <div className="my-5">
            <div className="max-w-32 mb-5">
                <Select
                    onChange={(value) =>
                        setPaymentStatus(
                            !!value ? (value as PaymentStatus) : undefined
                        )
                    }
                    value={paymentStatus?.toString()}
                    label="Status"
                    className="flex flex-1 items-center gap-2 border border-gray-900 px-3 py-1 bg-transparent rounded-none text-gray-900"
                >
                    <Option value="">All</Option>
                    <Option value="Pending">Pending</Option>
                    <Option value="Completed">Completed</Option>
                    <Option value="Refunded">Refunded</Option>
                </Select>
            </div>
            {isLoading ? (
                <div className="flex justify-center">
                    <Loader />
                </div>
            ) : (
                <>
                    {!payments[0] && (
                        <div className="flex justify-center text-gray-900 font-bold">
                            No payments
                        </div>
                    )}
                    <PaymentTable
                        payments={payments}
                        end={end}
                        next={getMorePayments}
                        onPaymentRefunded={(id) =>
                            setPayments(
                                payments.map((payment) => {
                                    if (payment.id === id) {
                                        payment.status = "Refunded";
                                    }

                                    return payment;
                                })
                            )
                        }
                    />
                </>
            )}
        </div>
    );
}
