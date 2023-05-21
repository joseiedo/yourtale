import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useAddPost() {
    const {data, error, loading, handleRequest} = useRequest();

    async function addPost(token, {description, picture, isPrivate}) {
        return await handleRequest(
            axiosInstance.post(
                `${ROUTES_PREFIX.POSTS}`,
                {
                    description,
                    picture,
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


    return {data, error, loading, addPost}


}