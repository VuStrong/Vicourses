"use client";

import {
    Button,
    Card,
    CardBody,
    CardFooter,
    Dialog,
    Input,
    Option,
    Select,
    Typography,
} from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useState } from "react";
import toast from "react-hot-toast";

import { Payment } from "@/libs/types/payment";
import { refundPayment } from "@/services/api/payment";

export default function RefundModal({
    payment,
    open,
    close,
    onPaymentRefunded,
}: {
    payment?: Payment;
    open: boolean;
    close: () => void;
    onPaymentRefunded: (paymentId: string) => void;
}) {
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [reason, setReason] = useState<string>("");
    const [otherReason, setOtherReason] = useState<string>("");
    const { data: session } = useSession();

    const onSubmit = async () => {
        if (isSubmitting || !payment) return;

        setIsSubmitting(true);
        
        try {
            let refundReason = reason !== "Other" ? reason : otherReason;

            await refundPayment(
                payment.id,
                session?.accessToken || "",
                refundReason
            );

            onPaymentRefunded(payment.id);
            close();
            toast.success("Payment has been refunded");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsSubmitting(false);
    };

    return (
        <Dialog
            size="lg"
            open={open}
            handler={close}
            className="bg-transparent shadow-none"
        >
            <Card className="mx-auto w-full max-w-[24rem]">
                <CardBody className="flex flex-col gap-4">
                    <Typography variant="h4" color="blue-gray">
                        Refund
                    </Typography>
                    <div>
                        <Typography
                            className="font-bold mb-1"
                            color="blue-gray"
                        >
                            Refund method
                        </Typography>

                        {payment?.method === "Paypal" && (
                            <Typography
                                className="font-bold border border-gray-900 p-2"
                                color="blue-gray"
                            >
                                ${payment.totalPrice} in Paypal
                            </Typography>
                        )}
                    </div>

                    <div>
                        <Select
                            label="Refund reason"
                            value={reason}
                            onChange={(value) => setReason(value || "")}
                        >
                            <Option value="This course is not good">
                                This course is not good
                            </Option>
                            <Option value="I don't want to learn anymore">
                                I don't want to learn anymore
                            </Option>
                            <Option value="Other">Other</Option>
                        </Select>
                    </div>

                    {reason === "Other" && (
                        <div>
                            <Input
                                crossOrigin={undefined}
                                label="Other reason"
                                value={otherReason}
                                onChange={(e) => setOtherReason(e.target.value)}
                            />
                        </div>
                    )}
                </CardBody>
                <CardFooter className="pt-0">
                    <Button
                        variant="gradient"
                        loading={isSubmitting}
                        onClick={onSubmit}
                        fullWidth
                    >
                        Submit
                    </Button>
                </CardFooter>
            </Card>
        </Dialog>
    );
}
