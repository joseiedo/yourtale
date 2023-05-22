import styles from "./FeedPhotos.module.css";
import React from "react";
import {FeedPhotosItem} from "../../feed/feedphotositem/FeedPhotosItem.component";
import {useGetPostsFromUser} from "../../../hooks/post/useGetPostsFromUser.hook";

export const FeedPhotosUser = ({userId, page, setModalPhoto, setInfinite}) => {
    const {data, getPostsFromUser} = useGetPostsFromUser();

    React.useEffect(() => {
        if (data?.isLastPage) {
            setInfinite(false);
        }
    }, [data])

    React.useEffect(() => {
        async function fetchPhotos() {
            const token = window.localStorage.getItem("token");
            getPostsFromUser(token, page, userId, 5);
        }

        fetchPhotos();
    }, [page]);

    return (
        <ul className={`${styles.feed} animeLeft`}>
            {data?.content.map((photo) => (
                <FeedPhotosItem
                    key={photo.id}
                    photo={photo}
                    setModalPhoto={setModalPhoto}
                />
            ))}
        </ul>

    );
};