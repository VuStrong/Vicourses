export type AdminDashboardData = {
    totalStudent: number;
    totalInstructor: number;
    totalPublishedCourse: number;
    totalMonthRevenue: number;
    metrics: AdminMetric[];
}

export type AdminMetric = {
    label: string;
    revenue: number;
}