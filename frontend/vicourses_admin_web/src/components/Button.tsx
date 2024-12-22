import { useMemo } from "react";

export default function Button({
    children,
    size = "sm",
    onClick,
    type,
    disabled,
    loading,
    className,
    variant = "button",
}: {
    children: React.ReactNode;
    size?: "sm" | "md" | "lg";
    onClick?: () => void;
    type?: "button" | "submit" | "reset";
    disabled?: boolean;
    loading?: boolean;
    className?: string;
    variant?: "button" | "text";
}) {
    const sizeStyle = useMemo(() => {
        if (size === "sm") {
            return "py-2 px-5";
        }
        if (size === "md") {
            return "py-3 px-8";
        }

        return "py-4 px-10";
    }, [size]);

    const variantStyle = useMemo(() => {
        if (variant === "text") {
            return "hover:opacity-50";
        }

        return "bg-primary text-white";
    }, [variant]);

    return (
        <button
            onClick={onClick}
            type={type}
            disabled={disabled || loading}
            className={`inline-flex items-center justify-center gap-2 rounded-md ${variantStyle} ${sizeStyle} text-center font-medium hover:bg-opacity-90 lg:px-8 xl:px-10 disabled:opacity-60 ${className}`}
        >
            {loading && (
                <div
                    className={`inline-block h-4 w-4 animate-spin rounded-full border-4 border-solid border-current border-r-transparent align-[-0.125em] motion-reduce:animate-[spin_1.5s_linear_infinite]`}
                    role="status"
                >
                    <span className="!absolute !-m-px !h-px !w-px !overflow-hidden !whitespace-nowrap !border-0 !p-0 ![clip:rect(0,0,0,0)]">
                        Loading...
                    </span>
                </div>
            )}
            {children}
        </button>
    );
}
