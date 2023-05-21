import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useEditPost() {
    const {data, error, loading, handleRequest} = useRequest();

    function editPost(token, {postId, isPrivate}) {
        return handleRequest(
            axiosInstance.put(
                `${ROUTES_PREFIX.POSTS}`,
                {
                    postId,
                    isPrivate
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, editPost}


}