import styles from './SearchUsers.module.css'
import {useGetUsersByText} from "../../hooks/user/useGetUsersByText.hook";
import React from "react";
import {usePagination} from "../../hooks/usePagination";
import {UserCard} from "./UserCard.component";

export const SearchUsers = () => {
    const {pages, setInfinite} = usePagination();
    const [search, setSearch] = React.useState('');

    return <>
        <section className={`container mainContainer ${styles.section}`}>
            <h1>Procure por algum usuário</h1>
            <input type="search" id="search" name="search" placeholder="Digite um nome ou email"
                   onChange={({target}) => setSearch(target.value)}
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

function UsersList({page, search, setInfinite}) {
    const {data, getUsersByText} = useGetUsersByText();
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

    return data?.content.map((user) => (
        <UserCard key={user.id} user={user}/>
    ))
}


