"use client";

import {
    PayPalButtons,
    PayPalScriptProvider,
    ReactPayPalScriptOptions,
} from "@paypal/react-paypal-js";
import toast from "react-hot-toast";
import { Session } from "next-auth";
import { useEffect, useRef } from "react";

import { Course } from "@/libs/types/course";
import { Payment } from "@/libs/types/payment";
import {
    cancelPayment,
    capturePaypalPayment,
    createPaypalPayment,
} from "@/services/api/payment";

const paypalOptions: ReactPayPalScriptOptions = {
    clientId: process.env.NEXT_PUBLIC_PAYPAL_CLIENT_ID || "",
};

export default function PaypalCheckoutButton({
    course,
    couponCode,
    session,
    onCompleted,
}: {
    course: Course;
    couponCode?: string;
    session: Session | null;
    onCompleted: () => void;
}) {
    const payment = useRef<Payment>();
    const accessToken = useRef<string>(session?.accessToken || "");

    const createOrder = async () => {
        try {
            const result = await createPaypalPayment(
                {
                    courseId: course.id,
                    couponCode,
                },
                accessToken.current
            );

            if (!result.paypalOrderId) {
                throw new Error("Paypal order id is missing");
            }

            payment.current = result;
            return result.paypalOrderId;
        } catch (error: any) {
            toast.error(error.message);
            throw error;
        }
    };

    const onApprove = async (data: any) => {
        try {
            await capturePaypalPayment(data.orderID, accessToken.current);

            onCompleted();
        } catch (error: any) {
            toast.error(error.message);
        }
    };

    const onCancel = () => {
        (async () => {
            if (!payment.current) return;

            try {
                await cancelPayment(payment.current.id, accessToken.current);
            } catch (error) {}

            toast.error("You have canceled the payment");
        })();
    };

    const onError = () => {
        if (payment.current) {
            cancelPayment(payment.current.id, accessToken.current).catch(
                () => undefined
            );
        }

        toast.error("An error has occured");
    };

    useEffect(() => {
        accessToken.current = session?.accessToken || "";
    }, [session?.accessToken]);

    if (!session) {
        return null;
    }

    return (
        <PayPalScriptProvider options={paypalOptions}>
            <PayPalButtons
                style={{ layout: "horizontal" }}
                createOrder={createOrder}
                onApprove={onApprove}
                onCancel={onCancel}
                onError={onError}
                forceReRender={[couponCode, course.id]}
            />
        </PayPalScriptProvider>
    );
}
