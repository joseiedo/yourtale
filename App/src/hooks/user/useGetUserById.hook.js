import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetUserById() {
    const {data, error, loading, handleRequest} = useRequest();

    function getUserById(token, userId) {
        return handleRequest(
            axiosInstance.get(
                `${ROUTES_PREFIX.USERS}/${userId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getUserById}


}