import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useGetPostDetails() {
    const {data, error, loading, handleRequest} = useRequest();

    function getPostDetails(token, postId) {
        return handleRequest(
            axiosInstance.get(
                `${ROUTES_PREFIX.POSTS}/details/${postId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, getPostDetails}


}