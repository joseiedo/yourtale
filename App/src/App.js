import './App.css';
import {Header} from "./components/global/header/Header.component";
import {Footer} from "./components/global/footer/Footer.component";
import {UserStorage} from "./UserContext";
import {Home} from "./components/home/Home.component";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {ROUTES} from "./router/routes";
import {Login} from "./components/user/Login.component";
import {ProtectedRoute} from "./components/helper/ProtectedRoute.component";
import {SearchUsers} from "./components/searchusers/SearchUsers.component";
import {SearchFriends} from "./components/searchusers/SearchFriends.component";
import {UserDetails} from "./components/userdetails/UserDetails.component";
import {UserRegister} from "./components/user/UserRegister.component";

function App() {
    return (
        <>
            <BrowserRouter>
                <UserStorage>
                    <Header/>
                    <div className="App">
                        <Routes>
                            <Route path={`${ROUTES.LOGIN}/*`} element={<Login/>}/>
                            <Route path={ROUTES.HOME} element={<ProtectedRoute><Home/></ProtectedRoute>}/>
                            <Route path={ROUTES.SEARCH_USER} element={<ProtectedRoute><SearchUsers/></ProtectedRoute>}/>
                            <Route path={`${ROUTES.PROFILE}/:userId`}
                                   element={<ProtectedRoute><UserDetails/></ProtectedRoute>}/>
                            <Route path={ROUTES.SEARCH_FRIENDS}
                                   element={<ProtectedRoute><SearchFriends/></ProtectedRoute>}/>
                            <Route path={ROUTES.REGISTER}
                                   element={<UserRegister/>}
                            />
                        </Routes>
                    </div>
                    <Footer/>
                </UserStorage>
            </BrowserRouter>
        </>
    );
}

export default App;
