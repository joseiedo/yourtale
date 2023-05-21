import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetFriends() {
    const {data, error, loading, handleRequest} = useRequest();

    function getFriends(token, text, page) {
        return handleRequest(
            axiosInstance.get(
                `${ROUTES_PREFIX.USERS}/friends/text=${text}&page=${page}&take=10`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getFriends}


}