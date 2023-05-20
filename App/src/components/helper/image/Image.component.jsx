import styles from './Image.module.css'
import React from "react";

export const Image = ({alt, ...props}) => {
    const [skeleton, setSkeleton] = React.useState(true)

    function handleLoad({target}) {
        setSkeleton(false)
        target.style.opacity = 1;
    }

    return (
        <div className={styles.wrapper}>
            {skeleton && <div className={styles.skeleton}></div>}
            <img alt={alt} {...props} className={styles.img} onLoad={handleLoad}/>
        </div>
    )
}