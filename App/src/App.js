import './App.css';
import {Header} from "./components/global/header/Header.component";
import {Footer} from "./components/global/footer/Footer.component";
import {UserStorage} from "./UserContext";
import {Home} from "./components/Home.component";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {ROUTES} from "./router/routes";
import {Login} from "./components/login/Login.component";

function App() {
    return (
        <>
            <BrowserRouter>
                <UserStorage>
                    <Header/>
                    <div className="App">
                        <Routes>
                            <Route path={ROUTES.LOGIN} element={<Login/>}/>
                            <Route path={ROUTES.HOME} element={<Home/>}/>
                        </Routes>
                    </div>
                    <Footer/>
                </UserStorage>
            </BrowserRouter>
        </>
    );
}

export default App;
