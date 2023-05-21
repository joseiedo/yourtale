import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useUnlikePost() {
    const {data, error, loading, handleRequest} = useRequest();

    function unlikePost(token, postId) {
        return handleRequest(
            axiosInstance.delete(
                `${ROUTES_PREFIX.POSTS}/${postId}/unlike`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, unlikePost}


}