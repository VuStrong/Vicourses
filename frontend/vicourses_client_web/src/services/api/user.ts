import { BACKEND_URL } from "@/libs/constants";
import { PublicProfile, UpdateProfileRequest, User } from "@/libs/types/user";

export async function getAuthenticatedUser(
    accessToken: string,
    fields?: string
): Promise<User | null> {
    let url = `${BACKEND_URL}/api/us/v1/me`;
    if (fields) {
        url += `?fields=${fields}`;
    }

    const res = await fetch(url, {
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

export async function updateProfile(request: UpdateProfileRequest, accessToken: string): Promise<User> {
    let url = `${BACKEND_URL}/api/us/v1/me`;

    const res = await fetch(url, {
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

export async function getPublicProfile(id: string, accessToken?: string): Promise<PublicProfile | null> {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/users/${id}/public-profile`, {
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

export async function linkPaypalAccount(code: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/me/paypal`, {
        method: "POST",
        body: JSON.stringify({ code }),
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

export async function changePassword(oldPassword: string, newPassword: string, accessToken: string) {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/me/password`, {
        method: "PATCH",
        body: JSON.stringify({ oldPassword, newPassword }),
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