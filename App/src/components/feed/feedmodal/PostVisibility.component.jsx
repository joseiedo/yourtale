import React from "react";
import {useEditPost} from "../../../hooks/post/useEditPosts.hook";

export function PostVisibility({privatePost, token, postId}) {
    const [isPrivate, setIsPrivatePost] = React.useState(privatePost);
    const {editPost} = useEditPost();

    React.useEffect(() => {
        if (isPrivate !== privatePost) {
            editPost(token, {postId, isPrivate})
        }
    }, [isPrivate]);


    return <div>
        <label htmlFor="switch">
            <input type="checkbox" id="switch" name="switch" role="switch" value={isPrivate} checked={isPrivate}
                   onChange={() => setIsPrivatePost(!isPrivate)}/>
            {isPrivate ? "Privado" : "Público"}
        </label>
    </div>
}