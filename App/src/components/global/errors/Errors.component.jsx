import {Error} from "../../helper/error/Error.component";

export function Errors({errors}) {
    if (!errors) return null;
    if ('errors' in errors.data) {
        return errors?.data.errors.map(({message}, index) => (
            <Error key={index} error={message}/>
        ));
    } else return null
}