import React from "react";
import {axiosInstance} from "./api/_base/axios-instance.api";
import {ROUTES_PREFIX} from "./api/_base/routes-prefix.api";

export const UserContext = React.createContext(null);

export const UserStorage = ({children}) => {
    const [data, setData] = React.useState(null);
    const [login, setLogin] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [error, setError] = React.useState(null);


    // const userLogout = React.useCallback(async function () {
    //         setData(null);
    //         setError(null);
    //         setLoading(false);
    //         setLogin(false);
    //         window.localStorage.removeItem("token");
    //         navigate("/login");
    //     },
    //     // [navigate]
    // );

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
            // navigate("/home");
        } catch (err) {
            console.log("ERR:", err)
            setError(err.response.data.errors[0].message);
            setLogin(false);
        } finally {
            setLoading(false);
        }

    }

    // React.useEffect(() => {
    //
    //     async function autoLogin() {
    //         const token = window.localStorage.getItem('token');
    //         if (token) {
    //             try {
    //                 setError(null);
    //                 setLoading(true);
    //                 // const { url, options } = TOKEN_VALIDATE_POST(token);
    //                 // const response = await fetch(url, options);
    //                 // if (!response.ok) throw new Error('Token inválido');
    //                 await getUser(token);
    //             } catch (err) {
    //                 userLogout();
    //             } finally {
    //                 setLoading(false);
    //             }
    //         } else {
    //             setLogin(false);
    //         }
    //     }
    //
    //     autoLogin();
    // }, [userLogout]);
    //
    return (
        <UserContext.Provider value={{userLogin, data, error, loading, login}}
        >
            {children}
        </UserContext.Provider>
    );
}

