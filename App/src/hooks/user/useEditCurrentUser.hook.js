import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useEditCurrentUser() {
    const {data, error, loading, handleRequest} = useRequest();

    function editCurrentUser(token, {
        userId,
        nickname,
        picture
    }) {
        return handleRequest(
            axiosInstance.put(
                `${ROUTES_PREFIX.USERS}/me`,
                {
                    userId,
                    nickname,
                    picture
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )
        )
    }


    return {data, error, loading, editCurrentUser}


}