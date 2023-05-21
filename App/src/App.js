import './App.css';
import {Header} from "./components/global/header/Header.component";
import {Footer} from "./components/global/footer/Footer.component";
import {UserStorage} from "./UserContext";
import {Home} from "./components/home/Home.component";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {ROUTES} from "./router/routes";
import {Login} from "./components/login/Login.component";
import {ProtectedRoute} from "./components/helper/ProtectedRoute.component";
import {SearchUsers} from "./components/searchusers/SearchUsers.component";

function App() {
    return (
        <>
            <BrowserRouter>
                <UserStorage>
                    <Header/>
                    <div className="App">
                        <Routes>
                            <Route path={ROUTES.LOGIN} element={<Login/>}/>
                            <Route path={ROUTES.HOME} element={<ProtectedRoute><Home/></ProtectedRoute>}/>
                            <Route path={ROUTES.SEARCH_USER} element={<ProtectedRoute><SearchUsers/></ProtectedRoute>}/>
                        </Routes>
                    </div>
                    <Footer/>
                </UserStorage>
            </BrowserRouter>
        </>
    );
}

export default App;
