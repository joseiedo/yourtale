import {useParams} from "react-router-dom";
import {useGetUserById} from "../../hooks/user/useGetUserById.hook";
import React from "react";
import styles from './UserDetails.module.css'
import {FeedModal} from "../feed/feedmodal/FeedModal.component";
import {usePagination} from "../../hooks/usePagination";
import {FeedPhotosUser} from "./userfeedphotos/FeedPhotosUser.component";
import {UserProfileCard} from "./UserProfileCard.component";

export function UserDetails() {
    const {userId} = useParams();
    const {data, getUserById, loading} = useGetUserById();
    const token = window.localStorage.getItem('token');
    const [modalPhoto, setModalPhoto] = React.useState(null);
    const {pages, setInfinite} = usePagination();

    React.useEffect(() => {
        getUserById(token, userId)
    }, [userId])

    React.useEffect(() => {
        console.log("USER DETAILS: ", data);
    }, [data])

    if (loading) {
        return <p aria-busy="true" className={styles.loading}>Carregando...</p>

    }
    return <section className={`container mainContainer ${styles.userDetailsSection}`}>
        <UserProfileCard data={data} getUserById={getUserById}/>
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
