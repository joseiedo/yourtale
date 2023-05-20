import styles from './Login.module.css';
import {Input} from "../forms/input/Input.component";

export const Login = () => {
    return (
        <section className={styles.login}>
            <div className={styles.forms}>
                <h1>Login</h1>
                <form>
                    <Input
                        error={""}
                        label={"Email"}
                        name={"email"}
                        type={"email"}
                    />
                    <Input
                        error={""}
                        label={"Senha"}
                        name={"password"}
                        type={"password"}
                    />
                    <button>Entrar</button>
                </form>
            </div>
        </section>
    )
}