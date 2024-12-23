import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { MdCheck } from "react-icons/md";
import { FaCcPaypal } from "react-icons/fa";
import { IoEyeOutline } from "react-icons/io5";
import Flatpickr from "react-flatpickr";
import "flatpickr/dist/themes/dark.css";

import { PagedResult } from "../../types/common";
import { Payment } from "../../types/payment";
import axiosInstance from "../../libs/axios";
import Loader from "../../components/Loader";
import { Select, Option } from "../../components/Forms";

const pageSize = 10;

export default function PaymentsPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [payments, setPayments] = useState<Payment[]>([]);
    const [total, setTotal] = useState<number>(0);

    const location = useLocation();
    const navigate = useNavigate();
    const queryParams = new URLSearchParams(location.search);

    const currentPage = Number(queryParams.get("page")) || 1;
    const totalPage = Math.ceil(total / pageSize);
    const paymentStatus = queryParams.get("status") || "";
    const fromDate = queryParams.get("from") || "";
    const toDate = queryParams.get("to") || "";

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<PagedResult<Payment>>(`/api/ps/v1/payments`, {
                params: {
                    skip: (currentPage - 1) * pageSize,
                    limit: pageSize,
                    status: paymentStatus || undefined,
                    from: fromDate || undefined,
                    to: toDate || undefined,
                },
            })
            .then((response) => {
                setPayments(response.data.items);
                setTotal(response.data.total);
                setIsLoading(false);
            });
    }, [currentPage, paymentStatus, fromDate, toDate]);

    const handleChangePage = (page: number) => {
        if (page === currentPage) return;

        queryParams.set("page", `${page}`);
        navigate({ search: queryParams.toString() });
    };

    const handleChangeStatus = (status: string) => {
        if (status === paymentStatus) return;

        if (status) {
            queryParams.set("status", status);
        } else {
            queryParams.delete("status");
        }

        queryParams.delete("page");
        navigate({ search: queryParams.toString() });
    };

    const handleChangeDateRange = (from: Date, to: Date) => {
        const fromStr = `${from.getFullYear()}-${from.getMonth() + 1}-${from.getDate()}`;
        const toStr = `${to.getFullYear()}-${to.getMonth() + 1}-${to.getDate()}`;
        
        if (fromStr === fromDate && toStr === toDate) return;

        queryParams.set("from", fromStr);
        queryParams.set("to", toStr);
        queryParams.delete("page");
        navigate({ search: queryParams.toString() });
    }

    return (
        <>
            <div className="flex items-center justify-between flex-wrap">
                <h1 className="text-2xl font-bold mb-5">Payments ({total})</h1>
            </div>
            <div className="flex gap-3 flex-col md:flex-row mb-5">
                <Select
                    value={paymentStatus || ""}
                    onChange={(e) => handleChangeStatus(e.target.value)}
                    disabled={isLoading}
                >
                    <Option value="">Select status</Option>
                    <Option value="Pending">Pending</Option>
                    <Option value="Completed">Completed</Option>
                    <Option value="Refunded">Refunded</Option>
                </Select>

                <Flatpickr
                    options={{
                        mode: "range",
                        dateFormat: "Y-m-d"
                    }}
                    value={[fromDate, toDate]}
                    onChange={([from, to]) => {
                        if (from && to) handleChangeDateRange(from, to);
                    }}
                    placeholder="Filter date"
                    className="dark:bg-form-input px-4 py-3"
                />
            </div>
            <div>
                {isLoading && (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                )}

                {!isLoading &&
                    (payments.length === 0 ? (
                        <div className="text-center">No payment found</div>
                    ) : (
                        <>
                            <PaymentsTable payments={payments} />
                            <div className="flex mt-5">
                                <button
                                    disabled={currentPage <= 1}
                                    onClick={() =>
                                        handleChangePage(currentPage - 1)
                                    }
                                    className="flex items-center justify-center px-3 h-8 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 disabled:opacity-50"
                                >
                                    Previous
                                </button>
                                <button
                                    disabled={currentPage >= totalPage}
                                    onClick={() =>
                                        handleChangePage(currentPage + 1)
                                    }
                                    className="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 disabled:opacity-50"
                                >
                                    Next
                                </button>
                            </div>
                        </>
                    ))}
            </div>
        </>
    );
}

function PaymentsTable({ payments }: { payments: Payment[] }) {
    return (
        <div className="rounded-sm border border-stroke bg-white pt-2 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark xl:pb-1">
            <div className="max-w-full overflow-x-auto">
                <table className="w-full min-w-max table-auto text-left">
                    <thead>
                        <tr>
                            <th className="min-w-[220px] py-4 px-4 pl-10 font-medium text-black dark:text-white">
                                Date
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Email
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Course
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Total
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Status
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Method
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {payments.map((payment, index) => {
                            return (
                                <tr key={index}>
                                    <td className="p-4">
                                        <div className="flex gap-2 items-center">
                                            <Link
                                                title="View details"
                                                to={`/payments/${payment.id}`}
                                                className="hover:text-primary"
                                            >
                                                <IoEyeOutline />
                                            </Link>
                                            {new Date(
                                                payment.createdAt,
                                            ).toLocaleDateString()}
                                        </div>
                                    </td>
                                    <td className="p-4">{payment.email}</td>
                                    <td className="p-4">
                                        {payment.courseName}
                                    </td>
                                    <td className="p-4">
                                        ${payment.totalPrice}
                                    </td>
                                    <td className="p-4">
                                        {payment.status === "Completed" && (
                                            <div className="flex gap-2 items-center bg-green-600 text-white rounded-full px-4 py-2 w-fit font-semibold">
                                                <MdCheck />
                                                {payment.status}
                                            </div>
                                        )}
                                        {payment.status === "Pending" && (
                                            <div className="bg-orange-500 text-white rounded-full px-4 py-2 w-fit font-semibold">
                                                {payment.status}
                                            </div>
                                        )}
                                        {payment.status === "Refunded" && (
                                            <div className="bg-gray-700 text-white rounded-full px-4 py-2 w-fit font-semibold">
                                                {payment.status}
                                            </div>
                                        )}
                                    </td>
                                    <td className="p-4">
                                        {payment.method === "Paypal" && (
                                            <div className="text-blue-700">
                                                <FaCcPaypal size={40} />
                                            </div>
                                        )}
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
