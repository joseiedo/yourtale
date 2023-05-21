import {createBrowserRouter} from "react-router-dom";
import {ROUTES} from "./routes";
import {Login} from "../components/login/Login.component";
import {Feed} from "../components/feed/Feed.component";

export const router = createBrowserRouter([
    {
        path: ROUTES.LOGIN,
        element: <Login/>,
    },
    {
        path: ROUTES.HOME,
        element: <Feed/>,
    }
])