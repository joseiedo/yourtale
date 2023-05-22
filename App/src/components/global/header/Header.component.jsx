import styles from './Header.module.css';
import {UserContext} from "../../../UserContext";
import React from "react";
import {ReactComponent as UserLogo} from "../../../assets/userIcon.svg";
import {Link} from "react-router-dom";
import {ROUTES} from "../../../router/routes";

export const Header = () => {
    const {data} = React.useContext(UserContext);

    console.log(data)
    return <header className={styles.header}>
        <nav className="container">
            <ul>
                <li><Link to={ROUTES.SEARCH_USER}>Buscar contatos</Link></li>
                <li><Link to={"/"}>Meus amigos</Link></li>
                <li><Link to={"/"}>Feed</Link></li>
            </ul>
            <li className={styles.logo}><strong><em>yourtale</em></strong></li>
            <ul>
                <li className={styles.avatarIcon}>{data && <>
                    <span>{data.fullName}</span>
                    <UserLogo/>
                </>}</li>
            </ul>
        </nav>
    </header>
}