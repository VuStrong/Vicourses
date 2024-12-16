import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import {
    Course,
    CourseCheckResponse,
    CourseDetail,
    CreateCourseRequest,
    GetCoursesQuery,
    GetInstructorCoursesQuery,
    InstructorCurriculum,
    PublicCurriculum,
    SearchCoursesQuery,
    UpdateCourseRequest,
    UpdateCurriculumRequest,
} from "@/libs/types/course";

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

export async function getCourseById(
    id: string,
    accessToken?: string
): Promise<CourseDetail | null> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/courses/${id}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken || ""}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function searchCourses(
    query?: SearchCoursesQuery
): Promise<PagedResult<Course> | null> {
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

export async function getInstructorCourses(
    query: GetInstructorCoursesQuery,
    accessToken?: string
): Promise<PagedResult<Course> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/instructor-courses?${params}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken || ""}`,
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function getUserEnrolledCourses(
    query: {
        userId: string;
        skip?: number;
        limit?: number;
    },
    accessToken?: string
): Promise<PagedResult<Course> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/enrolled-courses?${params}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken || ""}`,
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function getPublicCurriculum(
    courseId: string,
    accessToken?: string
): Promise<PublicCurriculum | null> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/${courseId}/public-curriculum`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken || ""}`,
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function getInstructorCurriculum(
    courseId: string,
    accessToken: string
): Promise<InstructorCurriculum | null> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/${courseId}/instructor-curriculum`,
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

    const data = await res.json();
    return data;
}

export async function createCourse(
    request: CreateCourseRequest,
    accessToken: string
): Promise<Course> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/courses`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    if (!res.ok) {
        if (res.status === 403) {
            throw new Error("Forbidden");
        }

        const error = await res.json();
        throw new Error(error.message);
    }

    const data = await res.json();
    return data;
}

export async function deleteCourse(courseId: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/courses/${courseId}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function updateCourse(
    courseId: string,
    request: UpdateCourseRequest,
    accessToken: string
): Promise<Course> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/courses/${courseId}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    if (!res.ok) {
        if (res.status === 403) {
            throw new Error("Forbidden");
        }

        const error = await res.json();
        throw new Error(error.message);
    }

    const data = await res.json();
    return data;
}

export async function checkCourse(
    courseId: string
): Promise<CourseCheckResponse | null> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/${courseId}/check`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        }
    );

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function updateCurriculumOrder(
    courseId: string,
    request: UpdateCurriculumRequest,
    accessToken: string
) {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/${courseId}/curriculum`,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
            body: JSON.stringify(request),
        }
    );

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function checkEnroll(
    courseId: string,
    accessToken: string
): Promise<boolean> {
    const res = await fetch(
        `${BACKEND_URL}/api/cs/v1/courses/${courseId}/enroll`,
        {
            method: "HEAD",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }
    );

    if (!res.ok) {
        return false;
    }

    return true;
}
