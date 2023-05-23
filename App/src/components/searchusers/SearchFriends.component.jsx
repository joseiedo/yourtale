import styles from './SearchUsers.module.css'
import React from "react";
import { usePagination } from "../../hooks/usePagination";
import { UserCard } from "./UserCard.component";
import { useGetFriends } from "../../hooks/user/useGetFriends.hook";

export const SearchFriends = () => {
    const { pages, setInfinite } = usePagination();
    const [search, setSearch] = React.useState('');

    return <>
        <section className={`container mainContainer ${styles.section}`}>
            <h1>Procure por algum amigo</h1>
            <input type="search" id="search" name="search" placeholder="Digite um nome ou email"
                onChange={({ target }) => setSearch(target.value)}
                value={search}
            />
            <ul className={` animeLeft`}>
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
            </ul>
        </section>
    </>

}

function UsersList({ page, search, setInfinite }) {
    const { data, getFriends } = useGetFriends();
    const token = window.localStorage.getItem('token');


    React.useEffect(() => {

        getFriends(token, search, page)

    }, [page, search])

    React.useEffect(() => {
        if (data?.isLastPage) {
            setInfinite(false);
        }
    }, [data])

    return data?.content.map((user) => (
        <UserCard key={user.id} user={user} />
    ))
}


