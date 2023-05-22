import styles from './SearchUsers.module.css'
import {useGetUsersByText} from "../../hooks/user/useGetUsersByText.hook";
import React from "react";
import {usePagination} from "../../hooks/usePagination";
import DefaultUserImage from "../../assets/default-picture.jpeg";

export const SearchUsers = () => {
    const {pages, setPages, setInfinite} = usePagination();
    const [search, setSearch] = React.useState('');

    return <>
        <section className={`container mainContainer ${styles.section}`}>
            <h1>Procure por algum usuário</h1>
            <input type="search" id="search" name="search" placeholder="Digite um nome ou email"
                   onChange={({target}) => setSearch(target.value)}
                   value={search}
            />
            {
                pages.map((page) => (
                    <UsersList
                        key={page}
                        page={page}
                        search={search}
                        setInfinite={setInfinite}
                    />
                ))
            }
        </section>
    </>

}

function UsersList({page, search, setInfinite}) {
    const {data, getUsersByText, loading} = useGetUsersByText();
    const token = window.localStorage.getItem('token');


    React.useEffect(() => {

        getUsersByText(token, search, page)

    }, [page, search])

    React.useEffect(() => {
        console.log(data)
        if (data?.isLastPage) {
            setInfinite(false);
        }
    }, [data])

    return <ul className={` animeLeft`}>
        {
            data?.content.map((user) => (
                <UserCard key={user.id} user={user}/>
            ))
        }
    </ul>
}


function UserCard({user}) {
    return <>
        <div className={styles.userCard}>
            <img src={user.picture || DefaultUserImage} alt={user.fullName}/>
            <div>
                <h2>{user.fullName}</h2>
                <p>{user.email}</p>
            </div>
        </div>
    </>
}