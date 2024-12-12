import { BACKEND_URL } from "@/libs/constants";
import { PagedResult } from "@/libs/types/common";
import {
    CreateRatingRequest,
    GetRatingsByCourseQuery,
    GetRatingsByInstructorQuery,
    Rating,
    UpdateRatingRequest,
} from "@/libs/types/rating";

export async function getRatingsByCourse(
    query: GetRatingsByCourseQuery,
    accessToken?: string
): Promise<PagedResult<Rating> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(`${BACKEND_URL}/api/rs/v1/ratings?${params}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken || ""}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    return await res.json();
}

export async function getRatingsByInstructor(
    query: GetRatingsByInstructorQuery,
    accessToken: string
): Promise<PagedResult<Rating> | null> {
    let params = "";

    Object.entries(query).forEach(([key, value]) => {
        if (value !== undefined) {
            params += `${key}=${value}&`;
        }
    });

    const res = await fetch(`${BACKEND_URL}/api/rs/v1/ratings/instructor?${params}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    return await res.json();
}

export async function createRating(
    request: CreateRatingRequest,
    accessToken: string
): Promise<Rating> {
    const res = await fetch(`${BACKEND_URL}/api/rs/v1/ratings`, {
        method: "POST",
        body: JSON.stringify(request),
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    const data = await res.json();

    if (!res.ok) throw new Error(data.message);

    return data;
}

export async function updateRating(
    ratingId: string,
    request: UpdateRatingRequest,
    accessToken: string
): Promise<Rating> {
    const res = await fetch(`${BACKEND_URL}/api/rs/v1/ratings/${ratingId}`, {
        method: "PATCH",
        body: JSON.stringify(request),
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    const data = await res.json();

    if (!res.ok) throw new Error(data.message);

    return data;
}

export async function deleteRating(ratingId: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/rs/v1/ratings/${ratingId}`, {
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

export async function respondRating(ratingId: string, response: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/rs/v1/ratings/${ratingId}/response`, {
        method: "POST",
        body: JSON.stringify({ response }),
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