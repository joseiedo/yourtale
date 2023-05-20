import React from "react";
import {Image} from "../../helper/image/Image.component";
import styles from "./FeedPhotosItem.module.css";

export const FeedPhotosItem = ({photo, setModalPhoto}) => {
    function handleClick() {
        setModalPhoto(photo);
    }

    return (
        <li className={styles.photo} onClick={handleClick}>
            <Image src={photo.picture} alt="ads"/>
        </li>
    );
};