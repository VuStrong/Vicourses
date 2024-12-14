import { BACKEND_URL } from "@/libs/constants";
import { CreateSectionRequest, Section, UpdateSectionRequest } from "@/libs/types/section";

export async function createSection(
    request: CreateSectionRequest,
    accessToken: string
): Promise<Section> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/sections`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function updateSection(
    sectionId: string,
    request: UpdateSectionRequest,
    accessToken: string
): Promise<Section> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/sections/${sectionId}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(request),
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function deleteSection(sectionId: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/sections/${sectionId}`, {
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
