import styles from './User.module.css';
import React from "react";
import {Input} from "../forms/input/Input.component";
import {useForm} from "../../hooks/useForm.hook";
import {UserContext} from "../../UserContext";
import {Error} from "../helper/error/Error.component";
import {Navigate, useNavigate} from "react-router-dom";
import {ROUTES} from "../../router/routes";

export const Login = () => {
    const {login, userLogin, error, loading} = React.useContext(UserContext);
    const navigate = useNavigate();
    const {formData, handleChange} = useForm({
        email: {
            value: '',
            error: ''
        },
        password: {
            value: '',
            error: ''
        }
    });


    function handleLogin(e) {
        e.preventDefault()
        if (formData.email.value && formData.password.value) {
            userLogin(formData.email.value, formData.password.value);
        }
    }


    if (login === true) return <Navigate to={ROUTES.HOME}/>
    return (
        <section className={styles.login}>
            <div className={styles.forms}>
                <h1>Login</h1>
                <Error error={error}/>
                <form onSubmit={handleLogin}>
                    <Input
                        error={""}
                        label={"Email"}
                        name={"email"}
                        type={"email"}
                        onChange={handleChange}
                    />
                    <Input
                        error={""}
                        label={"Senha"}
                        name={"password"}
                        type={"password"}
                        onChange={handleChange}
                    />
                    <button aria-busy={loading} className="contrast">Entrar</button>
                    <button className="contrast outline"
                            onClick={() => navigate(`${ROUTES.REGISTER}`)}>Cadastrar
                    </button>
                </form>
            </div>
        </section>
    )
}