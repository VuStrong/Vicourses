"use client";

import { Loader } from "@/components/common";
import { Coupon } from "@/libs/types/coupon";
import { Course } from "@/libs/types/course";
import { getCoupons, updateCoupon } from "@/services/api/coupon";
import { Card, Switch, Typography } from "@material-tailwind/react";
import { Session } from "next-auth";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";

export default function CouponActiveSection({ course }: { course: Course }) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [coupon, setCoupon] = useState<Coupon | null>(null);
    const { data: session, status } = useSession();

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getCoupons(
                    {
                        courseId: course.id,
                        isExpired: false,
                        limit: 1,
                    },
                    session.accessToken
                );

                if (result) {
                    setCoupon(result.items[0] || null);
                }
                
                setIsLoading(false);
            })();
        }
    }, [status, course]);

    return (
        <section className="mt-10">
            <div className="text-black font-semibold mb-3">
                Coupon is active
            </div>
            <div>
                {isLoading ? (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                ) : coupon ? (
                    <CouponTable coupon={coupon} session={session} />
                ) : (
                    <div className="flex justify-center text-gray-900">
                        No coupon
                    </div>
                )}
            </div>
        </section>
    );
}

function CouponTable({
    coupon,
    session,
}: {
    coupon: Coupon;
    session: Session | null;
}) {
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [active, setActive] = useState<boolean>(coupon.isActive);

    const onToggle = async (checked: boolean) => {
        if (isUpdating || !session) return;

        setIsUpdating(true);

        await updateCoupon(
            coupon.code,
            {
                isActive: checked,
            },
            session.accessToken
        );

        setIsUpdating(false);
        setActive(checked);
    };

    return (
        <Card className="h-full w-full overflow-scroll">
            <table className="w-full min-w-max table-auto text-left">
                <thead>
                    <tr>
                        <th className="border-b border-blue-gray-100 bg-blue-gray-50 p-4">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal leading-none opacity-70"
                            >
                                Code
                            </Typography>
                        </th>
                        <th className="border-b border-blue-gray-100 bg-blue-gray-50 p-4">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal leading-none opacity-70"
                            >
                                Discount
                            </Typography>
                        </th>
                        <th className="border-b border-blue-gray-100 bg-blue-gray-50 p-4">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal leading-none opacity-70"
                            >
                                Expired At
                            </Typography>
                        </th>
                        <th className="border-b border-blue-gray-100 bg-blue-gray-50 p-4">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal leading-none opacity-70"
                            >
                                Remain
                            </Typography>
                        </th>
                        <th className="border-b border-blue-gray-100 bg-blue-gray-50 p-4">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal leading-none opacity-70"
                            >
                                Active
                            </Typography>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td className="p-4 border-b border-blue-gray-50">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal"
                            >
                                {coupon.code}
                            </Typography>
                        </td>
                        <td className="p-4 border-b border-blue-gray-50">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal"
                            >
                                {coupon.discount}%
                            </Typography>
                        </td>
                        <td className="p-4 border-b border-blue-gray-50">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal"
                            >
                                {new Date(
                                    coupon.expiryDate
                                ).toLocaleDateString()}
                            </Typography>
                        </td>
                        <td className="p-4 border-b border-blue-gray-50">
                            <Typography
                                variant="small"
                                color="blue-gray"
                                className="font-normal"
                            >
                                {coupon.remain} / {coupon.count}
                            </Typography>
                        </td>
                        <td className="p-4 border-b border-blue-gray-50">
                            <Switch
                                color="teal"
                                checked={active}
                                onChange={(e) => onToggle(e.target.checked)}
                                crossOrigin={undefined}
                            />
                        </td>
                    </tr>
                </tbody>
            </table>
        </Card>
    );
}
