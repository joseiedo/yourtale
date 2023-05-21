import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useAcceptFriend() {
    const {data, error, loading, handleRequest} = useRequest();

    function acceptFriend(token, friendRequestId) {
        return handleRequest(
            axiosInstance.put(
                `${ROUTES_PREFIX.USERS}/friend-requests/${friendRequestId}`,
                {
                    header: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, acceptFriend}


}