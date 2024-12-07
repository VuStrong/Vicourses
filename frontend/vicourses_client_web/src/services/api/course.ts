import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import { Course, CourseDetail, GetCoursesQuery, SearchCoursesQuery } from "@/libs/types/course";

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

export async function getCourseById(id: string, accessToken?: string): Promise<CourseDetail | null> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/courses/${id}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken || ''}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function searchCourses(query?: SearchCoursesQuery): Promise<PagedResult<Course> | null> {
    let params = "";

    if (query) {
        Object.entries(query).forEach(([key, value]) => {
            if (value !== undefined) {
                params += `${key}=${value}&`;
            }
        });
    }

    const res = await fetch(`${BACKEND_URL}/api/ss/v1/search?${params}`, {
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