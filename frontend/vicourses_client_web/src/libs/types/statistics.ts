export type Scope = "Week" | "Month" | "Year" | "All";

export type InstructorPerformance = {
    scope: Scope;
    totalEnrollmentCount: number;
    totalRefundCount: number;
    totalRevenue: number;
    metrics: {
        label: string;
        enrollmentCount: number;
        refundCount: number;
        revenue: number;
    }[];
}

export type GetInstructorPerformanceQuery = {
    scope?: Scope;
    courseId?: string;
}