import axios from "axios";
import { BACKEND_URL } from "../libs/contants";

const instance = axios.create({
    baseURL: BACKEND_URL,
    timeout: 10000,

});

instance.interceptors.request.use(async function (config) {
    const dataString = localStorage.getItem("token_data");

    if (!dataString) {
        return config;
    }

    const data = JSON.parse(dataString);
    const expiryTime = data.expiryTime;
    const accessToken = data.accessToken;
    const refreshToken = data.refreshToken;
    const userId = data.userId;

    if (!expiryTime) return config;

    const shouldRefresh = new Date(expiryTime) < new Date();

    if (shouldRefresh) {
        try {
            const response = await instance.post("/api/us/v1/auth/refresh-token", {
                refreshToken,
                userId,
            });

            const now = new Date();
            now.setMinutes(now.getMinutes() + 1);
            localStorage.setItem("token_data", JSON.stringify({
                refreshToken,
                userId,
                accessToken: response.data.accessToken,
                expiryTime: now,
            }));

            config.headers["Content-Type"] = "application/json";
            config.headers.Authorization = `Bearer ${response.data.accessToken}`
        } catch (error) {
            
        }
    }

    config.headers.Authorization = `Bearer ${accessToken}`

    return config;
}, function (error) {
    return Promise.reject(error)
});

export default instance;