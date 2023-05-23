import { useGetPostDetails } from "../../../hooks/post/useGetPostDetails.hook";
import React from "react";
import { Image } from "../../helper/image/Image.component";
import styles from './FeedModal.module.css'
import DefaultUserImg from "../../../assets/default-picture.jpeg"
import { useGetCurrentUser } from "../../../hooks/user/useGetCurrentUser.hook";
import { Like } from "./Like.component";
import { PostVisibility } from "./PostVisibility.component";
import { CommentPost, Comments } from "./Comments.component";


export const FeedModal = ({ photo, setModalPhoto }) => {
    const { data, getPostDetails } = useGetPostDetails();
    const { user, getCurrentUser } = useGetCurrentUser();
    const token = localStorage.getItem('token');


    React.useEffect(() => {
        getCurrentUser(token);
        getPostDetails(token, photo.id)
    }, []);

    return <dialog open={photo}>

        <article className={styles.modalWrapper}>

            <Image src={data?.post.picture} />
            <div>
                <div>
                    <button className="close" aria-label="Close" onClick={() => setModalPhoto(null)}></button>
                    <div className={styles.avatar}>
                        <img src={data?.post.author.picture || DefaultUserImg} alt="Foto do usuário" />
                        <div>
                            <p>{data?.post.author.fullName}</p>
                            <span>{data?.post.createdAt.split("T")[0]}</span>
                        </div>
                    </div>
                    {
                        data && user && data?.post.author.id === user.id &&
                        <PostVisibility privatePost={data?.post.isPrivate} postId={photo.id} token={token} />
                    }

                    <p>{data?.post.description}</p>
                </div>
                <Comments comments={data?.comments} />

                <div>
                    <Like isLiked={data?.post.isLiked}
                        token={token} postId={photo.id}
                        getPostDetails={getPostDetails}
                        likesQuantity={data?.likesQuantity} />

                    <CommentPost postId={photo.id} getPostDetails={getPostDetails} token={token} />
                </div>
            </div>
        </article>
    </dialog>
}









