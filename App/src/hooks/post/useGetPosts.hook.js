import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetPosts() {
    const {data, error, loading, handleRequest} = useRequest();

    function getPosts(token, page) {
        return handleRequest(
            axiosInstance.get(
                `${ROUTES_PREFIX.POSTS}?take=5&page=${page}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getPosts}


}