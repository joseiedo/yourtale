import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetFriendsRequests() {
    const {data, error, loading, handleRequest} = useRequest();

    function getFriendsRequests(token) {
        return handleRequest(
            axiosInstance.post(
                `${ROUTES_PREFIX.USERS}/friend-requests/`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getFriendsRequests}


}