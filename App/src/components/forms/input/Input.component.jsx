import {Error} from "../../helper/error/Error.component";

export const Input = ({label, type, name, value, onChange, error, onBlur, ...props}) => {
    return (
        <div>
            <label htmlFor={name}>
                {label}
            </label>
            <input
                {...props}
                id={name}
                name={name}
                type={type}
                value={value}
                onChange={onChange}
                onBlur={onBlur}
            />
            <Error error={error}/>
        </div>
    );
};