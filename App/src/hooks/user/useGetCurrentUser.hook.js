import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetCurrentUser() {
    const {data, error, loading, handleRequest} = useRequest();

    function getCurrentUser(token) {
        return handleRequest(
            axiosInstance.get(
                `${ROUTES_PREFIX.USERS}/me`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getCurrentUser}


}