export default function Option({
    children,
    value,
    disabled,
}: {
    children: React.ReactNode;
    value?: string | number | readonly string[];
    disabled?: boolean;
}) {
    return (
        <option
            value={value}
            disabled={disabled}
            className="text-body dark:text-bodydark"
        >
            {children}
        </option>
    );
}
