import {Feed} from "../feed/Feed.component";
import {FriendsRequests} from "./FriendRequests.component";

export const Home = () => {

    return (
        <section className="container mainContainer">
            <FriendsRequests/>
            <Feed/>
        </section>
    )
}

