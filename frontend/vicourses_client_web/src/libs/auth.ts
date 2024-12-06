import NextAuth, { User } from "next-auth";
import Credentials from "next-auth/providers/credentials";
import Google from "next-auth/providers/google";
import { login, loginWithGoogle, refreshAccessToken } from "@/services/api/auth";
import { JWT } from "next-auth/jwt";

export const { handlers, signIn, signOut, auth } = NextAuth({
    providers: [
        Credentials({
            credentials: {
                email: {},
                password: {},
            },
            authorize: async (credentials) => {
                try {
                    if (!credentials?.email || !credentials.password) {
                        throw new Error("Email or password is missing");
                    }
                    const response = await login(
                        credentials.email as string,
                        credentials.password as string
                    );
    
                    return response as any;
                } catch (error) {
                    return null;                    
                }
            },
        }),
        Google,
    ],
    callbacks: {
        jwt: async ({ 
            token, user,
        }: { 
            token: JWT, 
            user: User, 
        }) => {
            if (user) {
                token.accessToken = user.accessToken;
                token.refreshToken = user.refreshToken;

                // set expire time to 25 minutes later
                const now = new Date();
                now.setMinutes(now.getMinutes() + 25);
                token.accessTokenExpiry = now;
                
                token.id = user.user.id;
            }
                       
            const shouldRefresh = new Date(token.accessTokenExpiry) < new Date();
            
            if (!shouldRefresh) {
                return Promise.resolve(token);
            }

            try {
                token.accessToken = (await refreshAccessToken(token.id, token.refreshToken)).accessToken;

                // set expire time to 25 minutes later
                const now = new Date();
                now.setMinutes(now.getMinutes() + 25);
                token.accessTokenExpiry = now;
            } catch (error) {
                token.error = "RefreshAccessTokenError";
            }

            return Promise.resolve(token);
        },
        session: async ({ session, token }) => {
            if (token) {
                session.user.id = token.id;

                session.accessToken = token.accessToken;
                session.refreshToken = token.refreshToken;
                session.accessTokenExpiry = token.accessTokenExpiry;
                session.error = token.error;
            }

            return Promise.resolve(session);
        },
        signIn: async ({ account, user }) => {
            if (account && account.provider === "google") {
                try {
                    const response = await loginWithGoogle(account.id_token || "");
                    
                    user.accessToken = response.accessToken;
                    user.refreshToken = response.refreshToken;
                    user.user = response.user;

                    return true;
                } catch (error: any) {
                    return false;                    
                }
            }
            
            return true;
        }
    },
    session: {
        strategy: "jwt",
    },
    pages: {
        signIn: "/login",
    },
});
