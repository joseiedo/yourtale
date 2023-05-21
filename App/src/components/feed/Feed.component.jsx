import {FeedModal} from "./feedmodal/FeedModal.component";
import {FeedPhotos} from "./feedphotos/FeedPhotos.component";
import React from "react";
import styles from "./feedphotos/FeedPhotos.module.css";
import {CreatePostModal} from "./createpostmodal/CreatePostModal.component";
import {useAddPost} from "../../hooks/post/useAddPosts.hook";

export const Feed = ({user}) => {
    const [modalPhoto, setModalPhoto] = React.useState(null);
    const [pages, setPages] = React.useState([1]);
    const [infinite, setInfinite] = React.useState(true);
    const [isAddingPhost, setIsAddingPost] = React.useState(false);
    const {addPost} = useAddPost();

    function handleCloseModal() {
        setIsAddingPost(false);
    }

    React.useEffect(() => {
        if (pages.length === 0) {
            setPages([1])
        }
    }, [pages]);


    React.useEffect(() => {
        let wait = false;

        function infiniteScroll() {
            if (infinite) {
                const scroll = window.scrollY;
                const height = document.body.offsetHeight - window.innerHeight;

                if (scroll > height * 0.75 && !wait) {
                    setPages((pages) => [...pages, pages.length + 1]);
                    wait = true;
                    setTimeout(() => {
                        wait = false;
                    }, 500);
                }
            }
        }

        window.addEventListener("wheel", infiniteScroll);
        window.addEventListener("scroll", infiniteScroll);
        return () => {
            window.removeEventListener("wheel", infiniteScroll);
            window.removeEventListener("scroll", infiniteScroll);
        };
    }, [infinite]);


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