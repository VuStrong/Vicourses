import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import { Course, GetCoursesQuery } from "@/libs/types/course";

export async function getCourses(
    query?: GetCoursesQuery
): Promise<PagedResult<Course> | null> {
    let params = "";

    if (query) {
        Object.entries(query).forEach(([key, value]) => {
            if (value !== undefined) {
                params += `${key}=${value}&`;
            }
        });
    }

    const res = await fetch(`${BACKEND_URL}/api/cs/v1/courses?${params}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}
