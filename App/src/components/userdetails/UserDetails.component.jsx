import {useParams} from "react-router-dom";
import {useGetUserById} from "../../hooks/user/useGetUserById.hook";
import React from "react";
import DefaultUserImage from '../../assets/default-picture.jpeg';
import styles from './UserDetails.module.css'
import {FeedModal} from "../feed/feedmodal/FeedModal.component";
import {usePagination} from "../../hooks/usePagination";
import {FeedPhotosUser} from "./feedphotos/FeedPhotosUser.component";

export function UserDetails() {
    const {userId} = useParams();
    const {data, getUserById} = useGetUserById();
    const token = window.localStorage.getItem('token');
    const [modalPhoto, setModalPhoto] = React.useState(null);
    const {pages, setInfinite} = usePagination();

    React.useEffect(() => {
        getUserById(token, userId)
    }, [userId])

    React.useEffect(() => {
        console.log("USER DETAILS: ", data);
    }, [data])

    return <section className={`container mainContainer ${styles.userDetailsSection}`}>
        <UserProfileCard data={data}/>
        {modalPhoto && (
            <FeedModal photo={modalPhoto} setModalPhoto={setModalPhoto}/>
        )}
        <div className="container mainContainer">

            {pages.map((page) => (
                <FeedPhotosUser
                    userId={userId}
                    key={page}
                    page={page}
                    setModalPhoto={setModalPhoto}
                    setInfinite={setInfinite}
                />
            ))}

        </div>
    </section>
}

function UserProfileCard({data}) {
    const Location = () => <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
        <path
            d="M18 0c-3.148 0-6 2.553-6 5.702 0 4.682 4.783 5.177 6 12.298 1.217-7.121 6-7.616 6-12.298 0-3.149-2.852-5.702-6-5.702zm0 8c-1.105 0-2-.895-2-2s.895-2 2-2 2 .895 2 2-.895 2-2 2zm-12-3c-2.099 0-4 1.702-4 3.801 0 3.121 3.188 3.451 4 8.199.812-4.748 4-5.078 4-8.199 0-2.099-1.901-3.801-4-3.801zm0 5.333c-.737 0-1.333-.597-1.333-1.333s.596-1.333 1.333-1.333 1.333.596 1.333 1.333-.596 1.333-1.333 1.333zm6 5.775l-3.215-1.078c.365-.634.777-1.128 1.246-1.687l1.969.657 1.92-.64c.388.521.754 1.093 1.081 1.75l-3.001.998zm12 7.892l-6.707-2.427-5.293 2.427-5.581-2.427-6.419 2.427 3.62-8.144c.299.76.554 1.776.596 3.583l-.443.996 2.699-1.021 4.809 2.091.751-3.725.718 3.675 4.454-2.042 3.099 1.121-.461-1.055c.026-.392.068-.78.131-1.144.144-.84.345-1.564.585-2.212l3.442 7.877z"/>
    </svg>

    const Email = () => <svg width="24" height="24" xmlns="http://www.w3.org/2000/svg" fill-rule="evenodd"
                             clip-rule="evenodd">
        <path
            d="M24 21h-24v-18h24v18zm-23-16.477v15.477h22v-15.477l-10.999 10-11.001-10zm21.089-.523h-20.176l10.088 9.171 10.088-9.171z"/>
    </svg>

    return <div className={styles.userCardWrapper}>
        <div className={`container mainContainer ${styles.userCard}`}>
            <img src={data?.user.picture || DefaultUserImage} alt={data?.user.fullName}/>
            <div className={styles.userInfo}>
                <hgroup>
                    <h1>{data?.user.fullName}</h1>
                    <p>@{data?.user.nickName}</p>
                </hgroup>
                <p><Location/> {data?.city} - {data?.uf}</p>
                <p><Email/> {data?.user.email}</p>
                <button className="outline contrast">Editar</button>
            </div>
        </div>
    </div>
}