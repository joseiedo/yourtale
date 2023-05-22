import {FeedModal} from "./feedmodal/FeedModal.component";
import {FeedPhotos} from "./feedphotos/FeedPhotos.component";
import React from "react";
import styles from "./feedphotos/FeedPhotos.module.css";
import {CreatePostModal} from "./createpostmodal/CreatePostModal.component";
import {useAddPost} from "../../hooks/post/useAddPosts.hook";
import {usePagination} from "../../hooks/usePagination";

export const Feed = ({user}) => {
    const [modalPhoto, setModalPhoto] = React.useState(null);
    const [isAddingPhost, setIsAddingPost] = React.useState(false);
    const {addPost} = useAddPost();
    const {pages, setPages, setInfinite} = usePagination();

    function handleCloseModal() {
        setIsAddingPost(false);
    }

    React.useEffect(() => {
        if (pages.length === 0) {
            setPages([1])
        }
    }, [pages]);


    async function handleCreatePost({
                                        description,
                                        picture,
                                        isPrivate
                                    }) {
        const token = localStorage.getItem('token');
        const response = await addPost(token, {
            description,
            picture,
            isPrivate
        });


        if (response.status === 201) {
            setInfinite(true);
            setPages([])
        }
    }


    return (
        <div>
            <button className={`${styles.button} contrast outline`} onClick={() => setIsAddingPost(true)}>NOVO POST +
            </button>
            <CreatePostModal isOpen={isAddingPhost} handleSubmit={handleCreatePost}
                             handleCloseModal={handleCloseModal}/>

            {modalPhoto && (
                <FeedModal photo={modalPhoto} setModalPhoto={setModalPhoto}/>
            )}
            {pages.map((page) => (
                <FeedPhotos
                    user={user}
                    key={page}
                    page={page}
                    setModalPhoto={setModalPhoto}
                    setInfinite={setInfinite}
                />
            ))}
        </div>
    );
}