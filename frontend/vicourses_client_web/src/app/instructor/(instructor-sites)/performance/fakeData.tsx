import { InstructorPerformance } from "@/libs/types/statistics";

export const performanceFake: InstructorPerformance = {
    scope: "Week",
    totalEnrollmentCount: 41,
    totalRefundCount: 0,
    totalRevenue: 122.99,
    metrics: [
        {
            label: "5/12/2024",
            enrollmentCount: 10,
            refundCount: 0,
            revenue: 49.99,
        },
        {
            label: "6/12/2024",
            enrollmentCount: 5,
            refundCount: 1,
            revenue: 29.99,
        },
        {
            label: "7/12/2024",
            enrollmentCount: 2,
            refundCount: 0,
            revenue: 9.99,
        },
        {
            label: "8/12/2024",
            enrollmentCount: 10,
            refundCount: 0,
            revenue: 49.99,
        },
        {
            label: "9/12/2024",
            enrollmentCount: 12,
            refundCount: 2,
            revenue: 79.99,
        },
        {
            label: "10/12/2024",
            enrollmentCount: 7,
            refundCount: 0,
            revenue: 39.99,
        },
        {
            label: "11/12/2024",
            enrollmentCount: 10,
            refundCount: 0,
            revenue: 49.99,
        },
    ],
};