import {Feed} from "../feed/Feed.component";
import {FriendsRequests} from "./FriendRequests.component";
import styles from './FriendRequests.module.css'

export const Home = () => {

    return (
        <section className={`container mainContainer ${styles.homeWrapper}`}>
            <FriendsRequests/>
            <Feed/>
        </section>
    )
}

