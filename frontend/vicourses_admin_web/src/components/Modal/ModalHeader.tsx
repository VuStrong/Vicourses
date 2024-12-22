export default function ModalHeader({
    children,
    className,
}: {
    children: React.ReactNode;
    className?: string;
}) {
    return (
        <div className={`px-4 py-3 sm:px-6 font-bold text-2xl ${className}`}>
            {children}
        </div>
    );
}
