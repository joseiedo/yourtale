import React from 'react';

const patterns = {
    cepNumber: /^\d{5}-?\d{3}$/,
    email: /^[\w+.]+@\w+\.\w{2,}(?:\.\w{2})?$/,
}

export function useForm(initialFormData) {
    const [formData, setFormData] = React.useState(initialFormData);

    function handleChange({target}) {
        const fieldValue = target.value;
        const fieldName = target.name;

        setFormData((pastInfo) => ({
            ...pastInfo,
            [fieldName]: {
                value: fieldValue,
                error: "",
            },
        }));

    }

    function validateForm() {
        const validFormData = Object.entries(formData).map(
            ([fieldName, fieldValue]) => {

                let patternError = "";
                if (
                    patterns[fieldName] &&
                    fieldValue.value.match(patterns[fieldName])
                ) {
                    patternError = "Campo inválido.";
                }

                return [
                    fieldName,
                    {
                        value: fieldValue.value,
                        error: patternError,
                    },
                ];
            }
        );

        const isEveryFieldValid = validFormData.every(([, field]) => {
            return field.error === "";
        });
        const formDataObj = Object.fromEntries(validFormData);
        setFormData(formDataObj);

        return isEveryFieldValid;
    }

    return {formData, handleChange, validateForm};
}