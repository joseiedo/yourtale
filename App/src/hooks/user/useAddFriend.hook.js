import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useAddFriend() {
    const {data, error, loading, handleRequest} = useRequest();

    function addFriend(token, friendId) {
        return handleRequest(
            axiosInstance.post(
                `${ROUTES_PREFIX.USERS}/friend-requests/${friendId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, addFriend}


}