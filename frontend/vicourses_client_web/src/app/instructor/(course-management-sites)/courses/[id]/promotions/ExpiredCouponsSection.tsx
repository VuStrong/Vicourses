"use client";

import { Loader } from "@/components/common";
import { Coupon } from "@/libs/types/coupon";
import { Course } from "@/libs/types/course";
import { getCoupons } from "@/services/api/coupon";
import { Button, Card, Typography } from "@material-tailwind/react";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";

const limit = 10;

export default function ExpiredCouponsSection({ course }: { course: Course }) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [coupons, setCoupons] = useState<Coupon[]>([]);
    const [skip, setSkip] = useState<number>(0);
    const [end, setEnd] = useState<boolean>(true);
    const { data: session, status } = useSession();

    const getMoreCoupons = async () => {
        if (status === "authenticated") {
            const result = await getCoupons(
                {
                    courseId: course.id,
                    isExpired: true,
                    skip: skip + limit,
                    limit,
                },
                session.accessToken
            );

            if (result) {
                setCoupons([...coupons, ...result.items]);
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

                const result = await getCoupons(
                    {
                        courseId: course.id,
                        isExpired: true,
                        limit,
                    },
                    session.accessToken
                );

                if (result) {
                    setCoupons(result.items);
                    setEnd(result.end);
                    setSkip(0);
                    setIsLoading(false);
                }
            })();
        }
    }, [status]);

    return (
        <section className="mt-10">
            <div className="text-black font-semibold mb-3">Expired coupons</div>
            <div>
                {isLoading ? (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                ) : (
                    <CouponTable
                        coupons={coupons}
                        end={end}
                        next={getMoreCoupons}
                    />
                )}
            </div>
        </section>
    );
}

function CouponTable({
    coupons,
    end,
    next,
}: {
    coupons: Coupon[];
    end: boolean;
    next: () => Promise<void>;
}) {
    const [isLoading, setIsLoading] = useState<boolean>(false);

    return (
        <>
            {!coupons[0] ? (
                <div className="flex justify-center text-gray-900">
                    No coupons
                </div>
            ) : (
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
                            </tr>
                        </thead>
                        <tbody>
                            {coupons.map((coupon, index) => {
                                const isLast = index === coupons.length - 1;
                                const classes = isLast
                                    ? "p-4"
                                    : "p-4 border-b border-blue-gray-50";

                                return (
                                    <tr key={coupon.code}>
                                        <td className={classes}>
                                            <Typography
                                                variant="small"
                                                color="blue-gray"
                                                className="font-normal"
                                            >
                                                {coupon.code}
                                            </Typography>
                                        </td>
                                        <td className={classes}>
                                            <Typography
                                                variant="small"
                                                color="blue-gray"
                                                className="font-normal"
                                            >
                                                {coupon.discount}%
                                            </Typography>
                                        </td>
                                        <td className={classes}>
                                            <Typography
                                                variant="small"
                                                color="blue-gray"
                                                className="font-normal"
                                            >
                                                {new Date(coupon.expiryDate).toLocaleDateString()}
                                            </Typography>
                                        </td>
                                        <td className={classes}>
                                            <Typography
                                                variant="small"
                                                color="blue-gray"
                                                className="font-normal"
                                            >
                                                {coupon.remain} / {coupon.count}
                                            </Typography>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </Card>
            )}
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
