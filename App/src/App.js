import './App.css';
import {Header} from "./components/global/header/Header.component";
import {Footer} from "./components/global/footer/Footer.component";
import {Home} from "./components/Home.component";

function App() {
    return (
        <>
            <Header/>
            <div className="App">
                <Home/>
                {/*<Login/>*/}
            </div>
            <Footer/>
        </>
    );
}

export default App;
