import styles from "./FeedPhotos.module.css";
import {CreatePostModal} from "../createpostmodal/CreatePostModal.component";
import React from "react";
import {useGetPosts} from "../../../hooks/post/useGetPosts.hook";
import {FeedPhotosItem} from "../feedphotositem/FeedPhotosItem.component";

export const FeedPhotos = ({page, user, setModalPhoto, setInfinite}) => {
    const [isAddingPhost, setIsAddingPost] = React.useState(false);
    const {data, error, loading, getPosts} = useGetPosts();

    React.useEffect(() => {
        if (data?.isLastPage) {
            setInfinite(false);
        }
    }, [data])
    //
    // React.useEffect(() => {
    //     getAllPassengers("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjIiLCJlbWFpbCI6InN0cmluZzNAZ21haWwuY29tIiwicm9sZSI6IlVTRVIiLCJuYmYiOjE2ODQ2MTk5MTcsImV4cCI6MTY4NDYyMzUxNywiaWF0IjoxNjg0NjE5OTE3fQ.w1v1Lwi1nPfAe_ryDtmIiVozUs0c9Ob_Zq-vxtSDK4g");
    // }, [])

    React.useEffect(() => {
        async function fetchPhotos() {
            getPosts("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjIiLCJlbWFpbCI6InN0cmluZzNAZ21haWwuY29tIiwicm9sZSI6IlVTRVIiLCJuYmYiOjE2ODQ2Mjk1NjQsImV4cCI6MTY4NDYzMzE2NCwiaWF0IjoxNjg0NjI5NTY0fQ.UYXTF2cowiXUsnlMhA8s_RqtctL274wsW0hLNS5C6wY");
        }

        fetchPhotos();
    }, [page, setInfinite]);
    //
    // if (error) return <Error error={error} />;
    // if (loading) return <Loading />;
    // if (data)

    function handleCloseModal() {
        setIsAddingPost(false);
    }

    return (
        <>
            <button className={`${styles.button} contrast outline`} onClick={() => setIsAddingPost(true)}>NOVO POST +
            </button>
            <CreatePostModal isOpen={isAddingPhost} handleCloseModal={handleCloseModal}/>
            <ul className={`${styles.feed} animeLeft`}>
                {data?.content.map((photo) => (
                    <FeedPhotosItem
                        key={photo.id}
                        photo={photo}
                        setModalPhoto={setModalPhoto}
                    />
                ))}
            </ul>

        </>
    );
};