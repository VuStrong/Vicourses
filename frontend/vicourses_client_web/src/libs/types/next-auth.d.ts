import NextAuth from "next-auth"

declare module "next-auth" {
    interface Session {
        user: SessionUser;
        accessToken: string;
        refreshToken: string;
        accessTokenExpiry: Date;
        error?: string;
    }
}

declare module "next-auth" {
    interface User {
        accessToken: string;
        refreshToken: string;
        user: {
            id: string;
        }
    }
}

declare module "next-auth/jwt" {
    interface JWT {
        accessToken: string;
        refreshToken: string;
        accessTokenExpiry: Date;
        error?: string;
        id: string;
    }
}

export type SessionUser = {
    id: string
}