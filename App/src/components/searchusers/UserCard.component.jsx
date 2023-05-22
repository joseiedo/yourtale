import styles from "./SearchUsers.module.css";
import DefaultUserImage from "../../assets/default-picture.jpeg";
import React from "react";
import {Link} from "react-router-dom";
import {ROUTES} from "../../router/routes";

export function UserCard({user}) {
    return <>
        <Link to={`${ROUTES.PROFILE}/${user.id}`} className={styles.link}>
            <div className={styles.userCard}>
                <img src={user.picture || DefaultUserImage} alt={user.fullName}/>
                <div>
                    <h2>{user.fullName}</h2>
                    <p>{user.email}</p>
                </div>
            </div>
        </Link>
    </>
}