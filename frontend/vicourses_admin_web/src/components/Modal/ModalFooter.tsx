export default function ModalFooter({
    children,
    className,
}: {
    children: React.ReactNode;
    className?: string;
}) {
    return (
        <div
            className={`px-4 py-3 sm:flex items-center justify-end sm:px-6 ${className}`}
        >
            {children}
        </div>
    );
}
