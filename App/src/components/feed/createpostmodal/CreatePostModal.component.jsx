import {Input} from "../../forms/input/Input.component";
import styles from './CreatePostModal.module.css'
import {useForm} from "../../../hooks/useForm.hook";
import {Image} from '../../helper/image/Image.component'

export const CreatePostModal = ({handleSubmit, isOpen, handleCloseModal}) => {
    const {formData, validateForm, handleChange} = useForm(
        {
            title: {
                value: '',
                error: ''
            },
            picture: {
                value: '',
                error: ''
            }
        }
    );

    return <dialog open={isOpen} className={styles.modalWrapper}>
        <article>
            <form onSubmit={handleSubmit}>
                <Input
                    label={"Titulo"}
                    name={"title"}
                    type={"text"}
                    value={formData.title.value}
                    onChange={handleChange}
                    error={formData.title.error}
                />
                <Input
                    label={"URL da Imagem"}
                    name={"picture"}
                    type={"text"}
                    value={formData.picture.value}
                    onChange={handleChange}
                    error={formData.picture.error}
                />
                {
                    formData.picture.value &&
                    <div className={styles.preview}>
                        <h4>Preview</h4>
                        <Image src={formData.picture.value}/>
                    </div>
                }
            </form>
            <footer>
                <button className="contrast outline" onClick={handleCloseModal}>Cancel</button>
                <button className="contrast">Save</button>
            </footer>

        </article>
    </dialog>
}