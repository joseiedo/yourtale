import React from "react";

export function useRequest() {
    const [data, setData] = React.useState(null);
    const [error, setError] = React.useState("");
    const [loading, setLoading] = React.useState(false);

    async function handleRequest(request) {
        try {
            setError(null);
            setLoading(true);
            const response = await request;
            setData(response.data);
            return response;
        } catch (error) {
            setError(error.response);
            return error
        } finally {
            setLoading(false);
        }
    }


    return {data, error, loading, handleRequest};
}
