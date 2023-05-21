import {useRequest} from "../useRequest.hook";
import {axiosInstance} from "../../api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "../../api/_base/routes-prefix.api";

export function useRegisterUser() {
    const {data, error, loading, handleRequest} = useRequest();

    function registerUser({fullName, email, nickname, birthDate, cep, password, confirmPassword, picture}) {
        return handleRequest(
            axiosInstance.post(
                `${ROUTES_PREFIX.USERS}/register`,
                {
                    fullName,
                    email,
                    nickname,
                    birthDate,
                    cep,
                    password,
                    confirmPassword,
                    picture
                }
            )
        )
    }


    return {data, error, loading, registerUser}


}