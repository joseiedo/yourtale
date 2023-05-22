import styles from './Header.module.css';
import {UserContext} from "../../../UserContext";
import React from "react";
import {ReactComponent as UserLogo} from "../../../assets/userIcon.svg";
import {Link} from "react-router-dom";
import {ROUTES} from "../../../router/routes";

export const Header = () => {
    const {data} = React.useContext(UserContext);

    return <header className={styles.header}>
        <nav className="container">
            {data &&
                <ul>
                    <li><Link to={ROUTES.SEARCH_USER}>Buscar contatos</Link></li>
                    <li><Link to={ROUTES.SEARCH_FRIENDS}>Meus amigos</Link></li>
                    <li><Link to={ROUTES.HOME}>Feed</Link></li>
                </ul>
            }
            <li className={styles.logo}><strong><em>yourtale</em></strong></li>
            <ul>
                <li>
                    {data && <Link to={`${ROUTES.PROFILE}/${data.id}`}>
                        <div className={styles.avatarIcon}>

                            <span>{data.fullName}</span>
                            <UserLogo/>

                        </div>
                    </Link>
                    }
                </li>
            </ul>
        </nav>
    </header>
}