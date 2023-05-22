import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useRemoveFriend() {
    const {data, error, loading, handleRequest} = useRequest();

    function removeFriend(token, friendRequestId) {
        return handleRequest(
            axiosInstance.delete(
                `${ROUTES_PREFIX.USERS}/friends/${friendRequestId}/remove`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, removeFriend}


}