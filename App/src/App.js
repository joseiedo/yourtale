import './App.css';
import {Header} from "./components/global/header/Header.component";
import {Footer} from "./components/global/footer/Footer.component";
import {UserStorage} from "./UserContext";
import {Login} from "./components/login/Login.component";

function App() {
    return (
        <>
            <Header/>
            <div className="App">
                <UserStorage>
                    {/*<Home/>*/}
                    <Login/>
                </UserStorage>
            </div>
            <Footer/>
        </>
    );
}

export default App;
