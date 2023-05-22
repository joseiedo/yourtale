import React from "react";
import {axiosInstance} from "./api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "./api/_base/routes-prefix.api";
import {useNavigate} from "react-router-dom";
import {ROUTES} from "./router/routes";

export const UserContext = React.createContext(null);

export const UserStorage = ({children}) => {
    const [data, setData] = React.useState(null);
    const [login, setLogin] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [error, setError] = React.useState(null);
    const navigate = useNavigate();

    const userLogout = async function () {
        setData(null);
        setError(null);
        setLoading(false);
        setLogin(false);
        window.localStorage.removeItem("token");
        navigate(ROUTES.HOME);
    }


    async function getUser(token) {
        const response = await axiosInstance.get(`${ROUTES_PREFIX.USERS}/me`,
            {headers: {Authorization: `Bearer ${token}`}});

        setData(response.data);
        setLogin(true);
    }

    async function userLogin(email, password) {
        try {
            setError(null);
            setLoading(true);
            const response = await axiosInstance.post(`${ROUTES_PREFIX.USERS}/login`, {
                email,
                password,
            })

            const {token} = response.data;
            window.localStorage.setItem('token', token);
            await getUser(token);
        } catch (err) {
            console.log("ERR:", err)
            setError(err.response.data.errors[0].message);
            setLogin(false);
        } finally {
            setLoading(false);
        }

    }

    React.useEffect(() => {

        async function autoLogin() {
            const token = window.localStorage.getItem('token');
            if (token) {
                try {
                    setError(null);
                    setLoading(true);
                    await getUser(token);
                } catch (err) {
                    userLogout();
                } finally {
                    setLoading(false);
                }
            } else {
                setLogin(false);
            }
        }

        autoLogin();
    }, []);

    return (
        <UserContext.Provider value={{userLogin, data, error, loading, login}}
        >
            {children}
        </UserContext.Provider>
    );
}

