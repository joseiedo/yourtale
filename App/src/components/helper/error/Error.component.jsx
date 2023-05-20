import styles from './Error.module.css';

export const Error = ({error}) => {

    if (!error) return null;
    return (
        <p className={styles.error}>{error}</p>
    )
}