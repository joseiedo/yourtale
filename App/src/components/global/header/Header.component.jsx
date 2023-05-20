import styles from './Header.module.css';

export const Header = () => {

    return <header className={styles.header}>
        <nav className="container">
            <ul>
                <li><a href="#" className="secondary">…</a></li>
            </ul>
            <ul>
                <li><strong><em>yourtale</em></strong></li>
            </ul>
            <ul>
                <li><a href="#" className="secondary">…</a></li>
            </ul>
        </nav>
    </header>
}