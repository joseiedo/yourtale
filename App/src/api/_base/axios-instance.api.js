import axios from "axios";

export const BASE_API_URL = "https://localhost:7294";

export const axiosInstance = axios.create({
    baseURL: BASE_API_URL,
    timeout: 10000,
});

