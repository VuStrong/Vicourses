import { BACKEND_URL } from "@/libs/constants";
import { RefreshTokenResponse, SignInResponse, User } from "@/libs/types/user";

export async function login(
    email: string,
    password: string
): Promise<SignInResponse> {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/login`, {
        method: "POST",
        body: JSON.stringify({ email, password }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data as SignInResponse;
}

export async function loginWithGoogle(idToken: string): Promise<SignInResponse> {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/google-login`, {
        method: "POST",
        body: JSON.stringify({ idToken }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data as SignInResponse;
}

export async function register(
    name: string,
    email: string,
    password: string
): Promise<{
    user: User
}> {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/register`, {
        method: "POST",
        body: JSON.stringify({ name, email, password }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data;
}

export async function refreshAccessToken(userId: string, refreshToken: string): Promise<RefreshTokenResponse> {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/refresh-token`, {
        method: "POST",
        body: JSON.stringify({ userId, refreshToken }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    const data = await res.json();

    if (!res.ok) {
        throw new Error(data.message);
    }

    return data as RefreshTokenResponse;
}

export async function revokeRefreshToken(userId: string, refreshToken: string) {
    await fetch(`${BACKEND_URL}/api/us/v1/auth/revoke-refresh-token`, {
        method: "POST",
        body: JSON.stringify({ userId, refreshToken }),
        headers: {
            "Content-Type": "application/json",
        },
    });
}

export async function confirmEmail(userId: string, token: string) {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/confirm-email`, {
        method: "POST",
        body: JSON.stringify({ userId, token }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function sendEmailConfirmationLink(email: string) {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/email-confirmation-link`, {
        method: "POST",
        body: JSON.stringify({ email }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function resetPassword(userId: string, token: string, newPassword: string) {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/reset-password`, {
        method: "POST",
        body: JSON.stringify({ userId, token, newPassword }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}

export async function sendPasswordResetLink(email: string) {
    const res = await fetch(`${BACKEND_URL}/api/us/v1/auth/password-reset-link`, {
        method: "POST",
        body: JSON.stringify({ email }),
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message);
    }
}