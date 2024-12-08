import { BACKEND_URL } from "@/libs/constants";
import { Wishlist } from "@/libs/types/wishlist";

export async function getWishlist(accessToken: string): Promise<Wishlist | null> {
    const res = await fetch(`${BACKEND_URL}/api/ws/v1/wishlist`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        return null;
    }

    const data = await res.json();
    return data;
}

export async function addToWishlist(courseId: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/ws/v1/wishlist/courses/${courseId}`, {
        method: "POST",
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

export async function removeFromWishlist(courseId: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/ws/v1/wishlist/courses/${courseId}`, {
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

export async function getCourseIdsInWishlist(accessToken: string): Promise<string[]> {
    const res = await fetch(`${BACKEND_URL}/api/ws/v1/wishlist/courses/ids`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
    });

    if (!res.ok) {
        return [];
    }

    const data = await res.json();
    return data.courseIds;
}