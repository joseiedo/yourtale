import {Input} from "../../forms/input/Input.component";
import styles from './CreatePostModal.module.css'
import {useForm} from "../../../hooks/useForm.hook";
import {Image} from '../../helper/image/Image.component'
import {useState} from "react";

export const CreatePostModal = ({handleSubmit, isOpen, handleCloseModal}) => {
    const [isPrivate, setIsPrivate] = useState(false);
    const {formData, handleChange} = useForm(
        {
            description: {
                value: '',
                error: ''
            },
            picture: {
                value: '',
                error: ''
            },
        }
    );

    function onSubmit(e) {
        e.preventDefault()
        handleSubmit({
            description: formData.description.value,
            picture: formData.picture.value,
            isPrivate
        });
    }

    return <dialog open={isOpen} className={styles.modalWrapper}>
        <article>
            <form onSubmit={onSubmit}>
                <Input
                    label={"Descrição"}
                    name={"description"}
                    type={"text"}
                    value={formData.description.value}
                    onChange={handleChange}
                    error={formData.description.error}
                    required
                />
                <Input
                    label={"URL da Imagem"}
                    required
                    name={"picture"}
                    type={"text"}
                    value={formData.picture.value}
                    onChange={handleChange}
                    error={formData.picture.error}
                />
                <fieldset>
                    <label htmlFor="terms">
                        <input type="checkbox" role="switch" value={isPrivate} onChange={() => setIsPrivate(!isPrivate)}
                               id="terms" name="terms"/>
                        Desejo deixar esse post privado
                    </label>
                </fieldset>
                {
                    formData.picture.value &&
                    <div className={styles.preview}>
                        <h4>Preview</h4>
                        <Image src={formData.picture.value}/>
                    </div>
                }

                <button className="contrast" type="submit">Postar</button>
                <button className="contrast outline" type="button" key="CANCEL" onClick={handleCloseModal}>Cancelar
                </button>
            </form>

        </article>
    </dialog>
}