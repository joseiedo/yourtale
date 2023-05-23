import { useCommentPost } from "../../../hooks/post/useCommentPost.hook";
import React from "react";
import styles from './FeedModal.module.css'
import DefaultImage from '../../../assets/default-picture.jpeg'

export function Comments({ comments }) {
    return <div className={styles.comments}>
        {comments?.map((comment) => (
            <div key={comment.id}>
                <img src={comment.user.picture || DefaultImage} alt={`foto de ${comment.user.fullName}`} />
                <div><b>{comment.user.fullName}</b> <p>{comment.description}</p></div>
            </div>))}
    </div>
}


export function CommentPost({ token, postId, getPostDetails }) {
    const { commentPost } = useCommentPost();
    const [comment, setComment] = React.useState("");


    async function handleSendComment(e) {
        e.preventDefault();
        if (comment.trim() === "") return;

        await commentPost(token, {
            postId, text: comment
        });
        setComment("");
        getPostDetails(token, postId);
    }


    return <form onSubmit={handleSendComment}>
        <input placeholder="Escreva um comentário..." value={comment}
            onChange={({ target }) => setComment(target.value)} />
    </form>
}