import styles from "./FeedPhotos.module.css";
import React from "react";
import {useGetPosts} from "../../../hooks/post/useGetPosts.hook";
import {FeedPhotosItem} from "../feedphotositem/FeedPhotosItem.component";

export const FeedPhotos = ({page, setModalPhoto, setInfinite}) => {
    const {data, getPosts} = useGetPosts();

    React.useEffect(() => {
        if (data?.isLastPage) {
            setInfinite(false);
        }
    }, [data])

    React.useEffect(() => {
        async function fetchPhotos() {
            const token = window.localStorage.getItem("token");
            getPosts(token, page);
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