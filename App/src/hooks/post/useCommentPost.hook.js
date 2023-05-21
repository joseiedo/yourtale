import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useCommentPost() {
    const {data, error, loading, handleRequest} = useRequest();

    function commentPost(token, {postId, text}) {
        return handleRequest(
            axiosInstance.post(
                `${ROUTES_PREFIX.POSTS}/comments`,
                {
                    postId,
                    text,
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, commentPost}


}