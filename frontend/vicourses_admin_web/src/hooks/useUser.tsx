import { create } from "zustand";
import { SignInResponse, User } from "../types/user";
import axiosInstance from "../libs/axios";
import { TOKEN_DATA_KEY } from "../libs/contants";

interface UserStore {
    status: "unauthenticated" | "authenticated" | "loading";
    data: User | null;

    initialize: () => Promise<void>;
    login: (data: SignInResponse) => Promise<void>;
    logout: () => Promise<void>;
}

const useUser = create<UserStore>((set) => ({
    status: "loading",
    data: null,

    initialize: async () => {
        const tokenData = localStorage.getItem(TOKEN_DATA_KEY);

        if (!tokenData) {
            return set({
                status: "unauthenticated",
                data: null,
            });
        }

        try {
            const res = await axiosInstance.get<User>("/api/us/v1/me");

            return set({
                status: "authenticated",
                data: res.data,
            });
        } catch (error) {
            return set({
                status: "unauthenticated",
                data: null,
            });
        }
    },

    login: async (data: SignInResponse) => {
        const expiryTime = new Date();
        expiryTime.setMinutes(expiryTime.getMinutes() + 25);

        localStorage.setItem(
            TOKEN_DATA_KEY,
            JSON.stringify({
                userId: data.user.id,
                accessToken: data.accessToken,
                refreshToken: data.refreshToken,
                expiryTime,
            }),
        );

        const res = await axiosInstance.get<User>("/api/us/v1/me");
        return set({
            status: "authenticated",
            data: res.data,
        });
    },
    
    logout: async () => {
        const tokenDataString = localStorage.getItem(TOKEN_DATA_KEY);

        if (tokenDataString) {
            const tokenData = JSON.parse(tokenDataString);
            localStorage.removeItem(TOKEN_DATA_KEY);

            axiosInstance.post("/api/us/v1/auth/revoke-refresh-token", {
                refreshToken: tokenData.refreshToken,
                userId: tokenData.userId,
            });
        }

        set({
            status: "unauthenticated",
            data: null,
        });
    }
}));

export default useUser;
