import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useLikePost() {
    const {data, error, loading, handleRequest} = useRequest();

    function likePost(token, postId) {
        return handleRequest(
            axiosInstance.post(
                `${ROUTES_PREFIX.POSTS}/${postId}/like`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, likePost}


}