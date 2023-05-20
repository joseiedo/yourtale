import {FeedModal} from "./feedmodal/FeedModal.component";
import {FeedPhotos} from "./feedphotos/FeedPhotos.component";
import React from "react";

export const Feed = ({user}) => {
    const [modalPhoto, setModalPhoto] = React.useState(null);
    const [pages, setPages] = React.useState([1]);
    const [infinite, setInfinite] = React.useState(true);

    return (
        <div>
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