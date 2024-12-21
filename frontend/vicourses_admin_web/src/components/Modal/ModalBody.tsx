export default function ModalBody({
    children,
    className,
}: {
    children: React.ReactNode;
    className?: string;
}) {
    return <div className={`px-4 py-5 sm:px-6 ${className}`}>{children}</div>;
}
