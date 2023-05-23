import styles from './Header.module.css';
import {UserContext} from "../../../UserContext";
import React from "react";
import {ReactComponent as UserLogo} from "../../../assets/userIcon.svg";
import {Link} from "react-router-dom";
import {ROUTES} from "../../../router/routes";

export const Header = () => {
    const {data, userLogout} = React.useContext(UserContext);

    return <header className={styles.header}>
        <nav className="container">
            {data &&
                <ul>
                    <li><Link to={ROUTES.SEARCH_USER}>Buscar contatos</Link></li>
                    <li><Link to={ROUTES.SEARCH_FRIENDS}>Meus amigos</Link></li>
                    <li><Link to={ROUTES.HOME}>Feed</Link></li>
                </ul>
            }
            <span>

                <h1><strong><em>yourtale</em></strong></h1>

            </span>
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
                <li>
                    {data && <button className="outline contrast" onClick={userLogout}>Logout</button>}
                </li>
            </ul>
        </nav>
    </header>
}