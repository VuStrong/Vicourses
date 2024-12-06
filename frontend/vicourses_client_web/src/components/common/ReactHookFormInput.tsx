"use client";

import { FieldErrors, UseFormRegisterReturn } from "react-hook-form";

interface InputProps {
    id: string;
    label: string;
    value?: string;
    type?: string;
    disabled?: boolean;
    register?: UseFormRegisterReturn;
    errors?: FieldErrors;
    isTextArea?: boolean;
}

const ReactHookFormInput: React.FC<InputProps> = ({
    id,
    label,
    value,
    type,
    disabled,
    register,
    errors,
    isTextArea,
}) => {
    return (
        <div className="w-full relative">
            {isTextArea ? (
                <textarea
                    id={id}
                    disabled={disabled}
                    {...register}
                    placeholder=" "
                    defaultValue={value}
                    className={`
                        peer
                        w-full
                        p-4
                        pt-6
                        bg-white
                        text-black
                        border-2
                        rounded-md
                        outline-none
                        transition
                        disabled:opacity-70
                        disabled:cursor-not-allowed
                        ${
                            errors && errors[id]
                                ? "border-error"
                                : "border-neutral-300"
                        }
                        ${
                            errors && errors[id]
                                ? "focus:border-error"
                                : "focus:border-black"
                        }
                    `}
                ></textarea>
            ) : (
                <input
                    id={id}
                    disabled={disabled}
                    {...register}
                    placeholder=" "
                    defaultValue={value}
                    type={type}
                    className={`
                        peer
                        w-full
                        p-4
                        pt-6
                        bg-white
                        text-black
                        border-2
                        rounded-md
                        outline-none
                        transition
                        disabled:opacity-70
                        disabled:cursor-not-allowed
                        ${
                            errors && errors[id]
                                ? "border-error"
                                : "border-neutral-300"
                        }
                        ${
                            errors && errors[id]
                                ? "focus:border-error"
                                : "focus:border-black"
                        }
                    `}
                />
            )}

            {errors && errors[id] && (
                <div className="text-error text-sm">
                    {errors[id]?.message as string}
                </div>
            )}

            <label
                htmlFor={id}
                className={`
                    absolute
                    text-md
                    duration-150
                    transform
                    -translate-y-3
                    top-5
                    left-3
                    origin-[0]
                    peer-placeholder-shown:scale-100
                    peer-placeholder-shown:translate-y-0
                    peer-focus:scale-75
                    peer-focus:-translate-y-4  
                    ${errors && errors[id] ? "text-error" : "text-black"}
                `}
            >
                {label}
            </label>
        </div>
    );
};

export default ReactHookFormInput;