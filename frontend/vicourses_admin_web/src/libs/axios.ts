import axios from "axios";
import { BACKEND_URL, TOKEN_DATA_KEY } from "./contants";

const axiosInstance = axios.create({
    baseURL: BACKEND_URL,
    timeout: 10000,
    headers: {
        "Content-Type": "application/json",
    },
});

axiosInstance.interceptors.request.use(async (config) => {
    const tokenDataString = localStorage.getItem(TOKEN_DATA_KEY);

    if (!tokenDataString) {
        return config;
    }

    const data = JSON.parse(tokenDataString);
    const { expiryTime, accessToken, refreshToken, userId } = data;

    const shouldRefresh = expiryTime ? new Date(expiryTime) < new Date() : true;

    if (shouldRefresh) {
        try {
            const response = await axios.post(
                `${BACKEND_URL}/api/us/v1/auth/refresh-token`,
                {
                    refreshToken,
                    userId,
                },
            );

            const newExpiryTime = new Date();
            newExpiryTime.setMinutes(newExpiryTime.getMinutes() + 25);

            localStorage.setItem(
                TOKEN_DATA_KEY,
                JSON.stringify({
                    refreshToken,
                    userId,
                    accessToken: response.data.accessToken,
                    expiryTime: newExpiryTime,
                }),
            );

            config.headers.Authorization = `Bearer ${response.data.accessToken}`;
        } catch (error) {
            localStorage.removeItem(TOKEN_DATA_KEY);
        }
    } else {
        config.headers.Authorization = `Bearer ${accessToken}`;
    }

    return config;
});

export default axiosInstance;
