"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useSession } from "next-auth/react";
import toast from "react-hot-toast";

import { Course } from "@/libs/types/course";
import { CheckCouponResponse } from "@/libs/types/coupon";
import { checkCoupon } from "@/services/api/coupon";
import { checkEnroll } from "@/services/api/course";
import PaypalCheckoutButton from "./PaypalCheckoutButton";
import CouponInput from "./CouponInput";

export default function CheckoutSummary({ course }: { course: Course }) {
    const [coupon, setCoupon] = useState<CheckCouponResponse | null>(null);
    const [disabled, setDisabled] = useState<boolean>(false);
    const { data: session, status } = useSession();
    const router = useRouter();

    const handleCheckCoupon = async (code: string) => {
        if (status === "authenticated") {
            const result = await checkCoupon(
                code,
                course.id,
                session.accessToken
            );

            setCoupon(result);
        }
    };

    const handlePaymentCompleted = () => {
        let interval: NodeJS.Timeout;

        setDisabled(true);

        const pullTask = new Promise((resolve) => {
            interval = setInterval(async () => {
                if (!session) return;

                const enrolled = await checkEnroll(course.id, session.accessToken);
                
                if (enrolled) {
                    resolve(true);
                    clearInterval(interval);
                    router.push(`/learn/course/${course.id}`);
                }
            }, 2000);
        });

        toast.promise(pullTask, {
            loading: "Wait, don't leave. We are enrolling you...",
            success: <b>You are enrolled</b>,
            error: <b>Error.</b>
        });
    };

    return (
        <div className="w-full">
            <h2 className="text-black font-bold text-2xl mb-7">Summary</h2>

            <div className="flex justify-between text-gray-800">
                <div>List price:</div>
                <div>${course.price}</div>
            </div>
            <hr className="border border-gray-300 my-3" />
            {coupon && coupon.isValid && (
                <div className="flex justify-between text-gray-800 font-bold text-sm">
                    <div>Discount:</div>
                    <div>-{coupon.discount}%</div>
                </div>
            )}
            <div className="flex justify-between text-gray-800 font-bold text-lg">
                <div>Total price:</div>
                <div>
                    $
                    {coupon && coupon.isValid
                        ? coupon.discountPrice
                        : course.price}
                </div>
            </div>

            <div className="mt-5">
                <CouponInput onApply={handleCheckCoupon} />
                {coupon && !coupon.isValid && (
                    <p className="text-error text-sm">{coupon.details}</p>
                )}
            </div>

            <div className="mt-10">
                <div className="text-xs text-gray-700 mb-2">
                    By completing your purchase, you agree to these{" "}
                    <Link href="#" className="text-primary font-semibold">
                        Terms of Service.
                    </Link>
                </div>
                {!disabled && (
                    <PaypalCheckoutButton
                        course={course}
                        session={session}
                        couponCode={
                            coupon && coupon.isValid ? coupon.code : undefined
                        }
                        onCompleted={handlePaymentCompleted}
                    />
                )}
            </div>

            <div className="mt-5">
                <div className="text-gray-800 font-bold text-center">
                    2 days money-back guarantee
                </div>
                <div className="text-center whitespace-normal text-sm text-gray-700">
                    You are not satisfied? Get a full refund within 2 days. No
                    need to ask why!
                </div>
            </div>
        </div>
    );
}
