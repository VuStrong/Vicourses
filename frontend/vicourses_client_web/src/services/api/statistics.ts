import { BACKEND_URL } from "@/libs/constants";
import {
    GetInstructorPerformanceQuery,
    InstructorPerformance,
} from "@/libs/types/statistics";

export async function getInstructorPerformance(
    query: GetInstructorPerformanceQuery,
    accessToken: string
): Promise<InstructorPerformance | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(
        `${BACKEND_URL}/api/stats/v1/instructor/performance?${params}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    return await res.json();
}
