import { BACKEND_URL } from "@/libs/constants";
import { Category } from "@/libs/types/category";

export async function getCategories(query?: {
    q?: string;
    parentId?: string;
}): Promise<Category[]> {
    const params = new URLSearchParams({
        ...query as any
    });

    const res = await fetch(`${BACKEND_URL}/api/cs/v1/categories?${params}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        return [];
    }

    const data = await res.json();
    return data;
}

export async function getNavigationCategories(): Promise<Category[]> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/categories/navigation-list`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        return [];
    }

    const data = await res.json();
    return data;
}

export async function getCategoryBySlug(slug: string): Promise<Category | null> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/categories/${slug}`, {
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