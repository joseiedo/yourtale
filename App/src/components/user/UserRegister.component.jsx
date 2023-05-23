import styles from './User.module.css';
import React from "react";
import {Input} from "../forms/input/Input.component";
import {useForm} from "../../hooks/useForm.hook";
import {UserContext} from "../../UserContext";
import {Navigate, useNavigate} from "react-router-dom";
import {ROUTES} from "../../router/routes";
import {Errors} from "../global/errors/Errors.component";
import {useRegisterUser} from "../../hooks/user/useRegisterUser.hook";
import {Image} from "../helper/image/Image.component";

export function UserRegister() {
    const {login, userLogin} = React.useContext(UserContext);
    const navigate = useNavigate();
    const {data, registerUser, error, loading} = useRegisterUser();
    const {formData, validateForm, handleChange} = useForm({
        fullName: {
            value: '',
            error: ''
        },
        email: {
            value: '',
            error: ''
        },
        nickname: {
            value: '',
            error: ''
        },
        birthDate: {
            value: '',
            error: ''
        },
        cep: {
            value: '',
            error: ''
        },
        password: {
            value: '',
            error: ''
        },
        confirmPassword: {
            value: '',
            error: ''
        },
        picture: {
            value: '',
            error: ''
        }
    });

    React.useEffect(() => {
        window.scrollTo({top: 0, left: 0, behavior: 'smooth'});
    }, [error])

    React.useEffect(() => {
        if (data) {
            userLogin(formData.email.value, formData.password.value);
        }
    }, [data])

    function handleRegister(e) {
        e.preventDefault()

        registerUser({
            fullName: formData.fullName.value,
            email: formData.email.value,
            nickname: formData.nickname.value,
            birthDate: formData.birthDate.value,
            cep: formData.cep.value,
            password: formData.password.value,
            confirmPassword: formData.confirmPassword.value,
            picture: formData.picture.value
        });

    }

    if (login === true) return <Navigate to={ROUTES.HOME}/>
    return (
        <section className={styles.register}>
            <div className={styles.forms}>
                <h1>Crie sua conta</h1>
                <Errors errors={error}/>
                <form onSubmit={handleRegister}>
                    <Input
                        label={"Nome Completo"}
                        name={"fullName"}
                        type={"text"}
                        onChange={handleChange}
                        maxLength={255}
                        required
                    />
                    <Input
                        error={formData.email.error}
                        label={"Email"}
                        name={"email"}
                        type={"email"}
                        onChange={handleChange}
                        maxLength={255}
                        required
                    />
                    <Input
                        label={"Apelido"}
                        name={"nickname"}
                        type={"text"}
                        onChange={handleChange}
                        maxLength={50}
                    />
                    <Input
                        error={""}
                        label={"Data de Nascimento"}
                        name={"birthDate"}
                        type={"date"}
                        onChange={handleChange}
                        required
                    />
                    <Input
                        error={error?.data?.errors?.Cep}
                        label={"CEP"}
                        name={"cep"}
                        type={"text"}
                        onChange={handleChange}
                        maxLength={8}
                        required
                    />
                    <Input
                        label={"Senha"}
                        name={"password"}
                        type={"password"}
                        onChange={handleChange}
                        maxLength={128}
                        required
                    />
                    <Input
                        label={"Confirme sua senha"}
                        name={"confirmPassword"}
                        error={error?.data?.errors?.ConfirmPassword}
                        type={"password"}
                        onChange={handleChange}
                        maxLength={128}
                        required
                    />
                    <Input
                        label={"Foto de perfil"}
                        name={"picture"}
                        type={"text"}
                        onChange={handleChange}
                        maxLength={512}
                    />
                    {formData.picture.value
                        &&
                        <div className={styles.preview}>
                            <Image src={formData.picture.value} alt={"Preview da foto do usuário"}/>
                        </div>
                    }
                    <button aria-busy={loading} className="contrast">Criar Conta</button>
                </form>
            </div>
        </section>
    )
}