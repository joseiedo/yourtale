import styles from "./SearchUsers.module.css";
import DefaultUserImage from "../../assets/default-picture.jpeg";
import React from "react";

export function UserCard({user}) {
    return <>
        <div className={styles.userCard}>
            <img src={user.picture || DefaultUserImage} alt={user.fullName}/>
            <div>
                <h2>{user.fullName}</h2>
                <p>{user.email}</p>
            </div>
        </div>
    </>
}