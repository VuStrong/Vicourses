import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import {
    DEFAULT_COURSE_IMAGE_URL,
    DEFAULT_USER_AVATAR_URL,
} from "../../libs/contants";
import { Payment } from "../../types/payment";
import { User } from "../../types/user";
import { Course } from "../../types/course";
import axiosInstance from "../../libs/axios";
import Loader from "../../components/Loader";

export default function PaymentDetailPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [payment, setPayment] = useState<Payment | null>(null);
    const [user, setUser] = useState<User | null>(null);
    const [course, setCourse] = useState<Course | null>(null);

    const params = useParams();

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<Payment>(`/api/ps/v1/payments/${params.id}`)
            .then((response) => {
                setPayment(response.data);
                setIsLoading(false);
            })
            .catch((error) => {
                if (error?.response?.status === 404) {
                    setIsLoading(false);
                }
            });
    }, [params.id]);

    useEffect(() => {
        if (!payment) return;

        axiosInstance
            .get<User>(`/api/us/v1/users/${payment.userId}`, {
                params: { fields: "thumbnailUrl" },
            })
            .then((response) => {
                setUser(response.data);
            });

        axiosInstance
            .get<Course>(`/api/cs/v1/courses/${payment.courseId}`)
            .then((response) => {
                setCourse(response.data);
            });
    }, [payment]);

    return (
        <>
            <h1 className="text-2xl font-bold mb-8">Payment Details</h1>

            <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                {isLoading && (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                )}

                {!isLoading && !payment && (
                    <div className="text-center">Payment not found</div>
                )}

                {!isLoading && payment && (
                    <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-10 mb-10">
                            <div>
                                <div className="text-xs mb-1">
                                    Payment Method
                                </div>
                                <div className="font-semibold">
                                    {payment.method}
                                </div>
                            </div>
                            <div>
                                <div className="text-xs mb-1">Date</div>
                                <div className="font-semibold">
                                    {new Date(payment.createdAt).toDateString()}
                                </div>
                            </div>
                            <div>
                                <div className="text-xs mb-1">Status</div>
                                <div className="font-semibold">
                                    {payment.status}
                                </div>
                            </div>
                            <div>
                                <div className="text-xs mb-1">
                                    Refund Due Date
                                </div>
                                <div className="font-semibold">
                                    {new Date(
                                        payment.refundDueDate,
                                    ).toDateString()}
                                </div>
                            </div>
                        </div>

                        <div className="md:flex gap-5">
                            <div className="mb-10 md:mb-0">
                                <div className="mb-10">
                                    <div className="font-bold mb-5">Buyer</div>
                                    <div className="flex gap-3">
                                        <div className="h-15 w-15 flex-shrink-0">
                                            <img
                                                src={
                                                    user?.thumbnailUrl ||
                                                    DEFAULT_USER_AVATAR_URL
                                                }
                                                alt={user?.name}
                                                className="w-full h-full rounded-full border"
                                            />
                                        </div>
                                        <div>
                                            <div className="font-bold text-lg">
                                                {payment.username}
                                            </div>
                                            <div className="text-sm">
                                                {payment.email}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <div className="font-bold mb-5">Course</div>
                                    <div className="flex gap-3">
                                        <div className="h-12.5 w-17 flex-shrink-0">
                                            <img
                                                src={
                                                    course?.thumbnailUrl ||
                                                    DEFAULT_COURSE_IMAGE_URL
                                                }
                                                alt={course?.title}
                                                className="w-full h-full object-cover"
                                            />
                                        </div>
                                        <div>
                                            <div className="font-bold text-lg">
                                                {payment.courseName}
                                            </div>
                                            <div className="text-sm">
                                                {course?.user.name}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div className="border p-5 rounded-md dark:border-gray-600 w-full md:w-[20rem]">
                                <div className="text-lg font-bold mb-5">
                                    Summary
                                </div>
                                <div className="flex justify-between">
                                    <div>List price:</div>
                                    <div>${payment.listPrice}</div>
                                </div>
                                <hr className="border dark:border-gray-600 my-3" />
                                <div className="flex justify-between font-bold text-sm">
                                    <div>Discount:</div>
                                    <div>-${payment.discount}</div>
                                </div>
                                <div className="flex justify-between font-bold text-lg">
                                    <div>Total price:</div>
                                    <div>${payment.totalPrice}</div>
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className="text-lg font-bold mb-3 mt-10">
                                Other information:
                            </div>
                            {payment.couponCode && (
                                <div>
                                    <span className="font-semibold">
                                        Coupon code:
                                    </span>{" "}
                                    {payment.couponCode}
                                </div>
                            )}
                            {payment.paypalOrderId && (
                                <div>
                                    <span className="font-semibold">
                                        Paypal Order ID:
                                    </span>{" "}
                                    {payment.paypalOrderId}
                                </div>
                            )}
                        </div>
                    </div>
                )}
            </div>
        </>
    );
}
