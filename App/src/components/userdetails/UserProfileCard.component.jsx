import styles from './UserDetails.module.css'
import DefaultUserImage from '../../assets/default-picture.jpeg'
import {Input} from "../forms/input/Input.component";
import {Image} from "../helper/image/Image.component";
import React from "react";
import {useEditCurrentUser} from "../../hooks/user/useEditCurrentUser.hook";
import {useAddFriend} from "../../hooks/user/useAddFriend.hook";
import {useRemoveFriendRequest} from "../../hooks/user/useRemoveFriendRequest.hook";
import {useAcceptFriend} from "../../hooks/user/useAcceptFriend.hook";

export function UserProfileCard({data, getUserById}) {
    const Location = () => <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
        <path
            d="M18 0c-3.148 0-6 2.553-6 5.702 0 4.682 4.783 5.177 6 12.298 1.217-7.121 6-7.616 6-12.298 0-3.149-2.852-5.702-6-5.702zm0 8c-1.105 0-2-.895-2-2s.895-2 2-2 2 .895 2 2-.895 2-2 2zm-12-3c-2.099 0-4 1.702-4 3.801 0 3.121 3.188 3.451 4 8.199.812-4.748 4-5.078 4-8.199 0-2.099-1.901-3.801-4-3.801zm0 5.333c-.737 0-1.333-.597-1.333-1.333s.596-1.333 1.333-1.333 1.333.596 1.333 1.333-.596 1.333-1.333 1.333zm6 5.775l-3.215-1.078c.365-.634.777-1.128 1.246-1.687l1.969.657 1.92-.64c.388.521.754 1.093 1.081 1.75l-3.001.998zm12 7.892l-6.707-2.427-5.293 2.427-5.581-2.427-6.419 2.427 3.62-8.144c.299.76.554 1.776.596 3.583l-.443.996 2.699-1.021 4.809 2.091.751-3.725.718 3.675 4.454-2.042 3.099 1.121-.461-1.055c.026-.392.068-.78.131-1.144.144-.84.345-1.564.585-2.212l3.442 7.877z"/>
    </svg>

    const Email = () => <svg width="24" height="24" xmlns="http://www.w3.org/2000/svg" fillRule="evenodd"
                             clipRule="evenodd">
        <path
            d="M24 21h-24v-18h24v18zm-23-16.477v15.477h22v-15.477l-10.999 10-11.001-10zm21.089-.523h-20.176l10.088 9.171 10.088-9.171z"/>
    </svg>

    return <div className={styles.userCardWrapper}>
        <div className={`container mainContainer ${styles.userCard}`}>
            <img src={data?.user.picture || DefaultUserImage} alt={data?.user.fullName}/>
            <div className={styles.userInfo}>
                <hgroup>
                    <h1>{data?.user.fullName}</h1>
                    <p>{data?.user.nickName}</p>
                </hgroup>
                <p><Location/> {data?.city} - {data?.uf}</p>
                <p><Email/> {data?.user.email}</p>
                <ProfileButtons isFriend={data?.isFriend}
                                isFriendRequestPending={data?.friendRequestPending}
                                friendshipId={data?.friendshipId}
                                isLoggedUser={data?.isLoggedUser}
                                user={data?.user}
                                getUserById={getUserById}
                                friendRequestReceived={data?.friendRequestReceived}
                />
            </div>
        </div>
    </div>
}

function ProfileButtons({
                            isFriend,
                            isLoggedUser,
                            isFriendRequestPending,
                            user,
                            getUserById,
                            friendRequestReceived,
                            friendshipId
                        }) {
    const [isEditing, setIsEditing] = React.useState(false);


    if (isLoggedUser) return <>
        <button className="outline contrast" onClick={() => setIsEditing(true)}>Editar</button>
        <EditingUserModal isOpen={isEditing} user={user} getUserById={getUserById}
                          handleCloseModal={() => setIsEditing(false)}/>
    </>
    if (isFriendRequestPending) return <button className="outline contrast">Cancelar solicitação</button>
    if (friendRequestReceived) return <FriendRequestButtons getUserById={getUserById} userId={user.id}
                                                            friendshipId={friendshipId}/>
    if (isFriend) return <button className="outline contrast">Remover amigo</button>
    return <SendFriendRequestButton getUserById={getUserById} userId={user?.id}/>
}

function FriendRequestButtons({getUserById, userId, friendshipId}) {
    const {acceptFriend} = useAcceptFriend();
    const {removeFriendRequest} = useRemoveFriendRequest()
    const token = localStorage.getItem('token');

    async function handleRemoveFriendRequest() {
        await removeFriendRequest(token, friendshipId);
        getUserById(token, userId)
    }

    async function handleAcceptFriendRequest() {
        await acceptFriend(token, friendshipId);
        getUserById(token, userId)
    }


    return <>
        <button className="outline contrast" onClick={handleAcceptFriendRequest}>
            Aceitar amizade
        </button>
        <button className="outline contrast" onClick={handleRemoveFriendRequest}>
            Recusar amizade
        </button>
    </>

}

function SendFriendRequestButton({getUserById, userId}) {
    const token = localStorage.getItem('token');
    const {addFriend} = useAddFriend();

    async function handleAddFriend() {
        await addFriend(token, userId);
        getUserById(token, userId);
    }

    return <button className="outline contrast" onClick={handleAddFriend}>Adicionar amigo</button>
}


function EditingUserModal({isOpen, user, handleCloseModal, getUserById}) {
    const [nickname, setNickname] = React.useState(user?.nickName)
    const [picture, setPicture] = React.useState(user?.picture)
    const {editCurrentUser} = useEditCurrentUser();
    const token = localStorage.getItem('token');

    async function onSubmit(e) {
        e.preventDefault()
        await editCurrentUser(token, {
            userId: user.id,
            nickname: nickname,
            picture: picture,
        });
        getUserById(token, user.id);
    }


    if (!isOpen) return null;


    return <dialog open={isOpen} className={styles.editUser}>
        <article>
            <form onSubmit={onSubmit}>
                <Input
                    label={"Apelido"}
                    name={"nickname"}
                    type={"text"}
                    value={nickname}
                    onChange={({target}) => setNickname(target.value)}
                />
                <Input
                    label={"URL da foto"}
                    name={"picture"}
                    type={"text"}
                    value={picture}
                    onChange={({target}) => setPicture(target.value)}
                />
                {
                    picture &&
                    <div className={styles.preview}>
                        <h4>Preview</h4>
                        <Image src={picture}/>
                    </div>
                }

                <button className="contrast" type="submit">Postar</button>
                <button className="contrast outline" type="button" key="CANCEL" onClick={handleCloseModal}>Cancelar
                </button>
            </form>

        </article>
    </dialog>
}
