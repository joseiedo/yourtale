import logo from './logo.svg';
import './App.css';
import {Header} from "./components/global/header/Header.component";
import {Login} from "./components/login/Login.component";
import {Footer} from "./components/global/footer/Footer.component";

function App() {
    return (
        <>
            <Header/>
            <Login/>
            <Footer/>
        </>
    );
}

export default App;
