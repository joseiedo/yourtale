import {FeedPhotosItem} from "../feedphotositem/FeedPhotosItem.component";
import styles from "./FeedPhotos.module.css";

export const FeedPhotos = ({page, user, setModalPhoto, setInfinite}) => {
    const data = [{id: 1, picture: 'https://source.unsplash.com/random/1000x1000?1'},
        {
            id: 3,
            picture: 'https://source.unsplash.com/random/1000x1000?3'
        },
        {id: 2, picture: 'https://source.unsplash.com/random/1000x1000?2'},
        {id: 4, picture: 'https://source.unsplash.com/random/1000x1000?4'},
        {id: 5, picture: 'https://source.unsplash.com/random/1000x1000?5'},
        {id: 6, picture: 'https://source.unsplash.com/random/1000x1000?6'}];

    // const { data, loading, error, request } = useFetch();

    // React.useEffect(() => {
    //     async function fetchPhotos() {
    //         const total = 6;
    //         const { url, options } = PHOTOS_GET({ page, total, user });
    //         const { response, json } = await request(url, options);
    //         if (response && response.ok && json.length < total) {
    //             setInfinite(false);
    //         }
    //     }
    //     fetchPhotos();
    // }, [request, user, page, setInfinite]);

    // if (error) return <Error error={error} />;
    // if (loading) return <Loading />;
    // if (data)

    return (
        <ul className={`${styles.feed} animeLeft`}>
            {data.map((photo) => (
                <FeedPhotosItem
                    key={photo.id}
                    photo={photo}
                    setModalPhoto={setModalPhoto}
                />
            ))}
        </ul>
    );
};