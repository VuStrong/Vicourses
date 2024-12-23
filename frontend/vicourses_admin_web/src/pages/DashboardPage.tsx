import { useEffect, useState } from "react";
import { FaUsers, FaMoneyBillWave } from "react-icons/fa";
import { MdOutlineOndemandVideo } from "react-icons/md";

import { AdminDashboardData } from "../types/statistics";
import axiosInstance from "../libs/axios";
import AdminRevenueChart from "../components/Charts/AdminRevenueChart";
import CardDataStats from "../components/CardDataStats";
import Loader from "../components/Loader";

export default function DashboardPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [data, setData] = useState<AdminDashboardData>();

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<AdminDashboardData>("/api/stats/v1/admin/dashboard")
            .then((response) => {
                setData(response.data);
                setIsLoading(false);
            });
    }, []);

    if (isLoading) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <>
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 xl:grid-cols-4 2xl:gap-7.5">
                <CardDataStats
                    title="Total Revenue This Month"
                    total={`$${data?.totalMonthRevenue}`}
                >
                    <FaMoneyBillWave />
                </CardDataStats>
                <CardDataStats
                    title="Total Students"
                    total={`${data?.totalStudent}`}
                >
                    <FaUsers />
                </CardDataStats>
                <CardDataStats
                    title="Total Instructors"
                    total={`${data?.totalInstructor}`}
                >
                    <FaUsers />
                </CardDataStats>
                <CardDataStats
                    title="Total Published Courses"
                    total={`${data?.totalPublishedCourse}`}
                >
                    <MdOutlineOndemandVideo />
                </CardDataStats>
            </div>

            <div className="mt-4 grid grid-cols-12 gap-4 md:mt-6 md:gap-6 2xl:mt-7.5 2xl:gap-7.5">
                <AdminRevenueChart initialMetrics={data?.metrics || []} />
            </div>
        </>
    );
}
