import styles from "./FriendRequests.module.css";
import {useGetFriendsRequests} from "../../hooks/user/useGetFriendsRequests.hook";
import React from "react";
import defaultPicture from '../../assets/default-picture.jpeg';
import {useAcceptFriend} from "../../hooks/user/useAcceptFriend.hook";
import {useRemoveFriendRequest} from "../../hooks/user/useRemoveFriendRequest.hook";

export function FriendsRequests() {
    const {loading, data, getFriendsRequests} = useGetFriendsRequests();
    const {acceptFriend} = useAcceptFriend();
    const {removeFriendRequest} = useRemoveFriendRequest();

    const token = localStorage.getItem("token");

    React.useEffect(() => {
        getFriendsRequests(token);
    }, []);

    React.useEffect(() => {
        console.log("DATA:", data)
    }, [data])

    async function handleRemoveFriendRequest(friendShipId) {
        await removeFriendRequest(token, friendShipId);
        await getFriendsRequests(token)
    }

    async function handleAcceptFriendRequest(friendShipId) {
        await acceptFriend(token, friendShipId);
        await getFriendsRequests(token)
    }

    if (data?.length === 0) return null

    return (
        <article className={styles.friendRequestWrapper}>
            <header>
                <span aria-busy={loading}>Solicitações de amizade</span>
            </header>
            <ul>
                {data?.map(({id, user}) => (
                    <li key={id}>
                        {user.fullName}
                        <div>
                            <img src={user.picture || defaultPicture} alt={`Foto de ${user.fullName}`}/>
                        </div>
                        <div>
                            <button
                                onClick={() => handleAcceptFriendRequest(id)}
                            >Aceitar
                            </button>
                            <button
                                onClick={() => handleRemoveFriendRequest(id)}
                            >Recusar
                            </button>
                        </div>
                    </li>))}
            </ul>
        </article>
    )
}