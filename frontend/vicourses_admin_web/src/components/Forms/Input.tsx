export default function Input({
    id,
    placeholder,
    value,
    onChange,
    type,
    disabled,
    error,
    ref,
}: {
    id?: string;
    placeholder?: string;
    value?: string | number | readonly string[];
    onChange?: React.ChangeEventHandler<HTMLInputElement>;
    type?: React.HTMLInputTypeAttribute;
    disabled?: boolean;
    error?: boolean;
    ref?: React.LegacyRef<HTMLInputElement>;
}) {
    return (
        <input
            id={id}
            placeholder={placeholder}
            value={value}
            onChange={onChange}
            type={type}
            disabled={disabled}
            ref={ref}
            className={`w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary ${
                error && "!border-red-700"
            }`}
        />
    );
}
