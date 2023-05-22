import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetPostsFromUser() {
    const {data, error, loading, handleRequest} = useRequest();

    function getPostsFromUser(token, page, userId, take = 6) {
        return handleRequest(
            axiosInstance.get(
                `${ROUTES_PREFIX.POSTS}/${userId}?take=${take}&page=${page}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getPostsFromUser}


}