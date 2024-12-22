export default function Modal({
    children,
    open,
    onClose,
}: {
    children: React.ReactNode;
    open: boolean;
    onClose: () => void;
}) {
    return (
        <div className={`relative z-[100000] ${!open && "hidden"}`}>
            <div className="fixed inset-0 bg-gray-900 bg-opacity-75 transition-opacity w-screen h-full"></div>
            <div
                className="fixed inset-0 z-50 w-screen overflow-y-auto"
                onClick={onClose}
            >
                <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                    <div
                        onClick={(e) => e.stopPropagation()}
                        className="transform rounded-lg text-left shadow-xl transition-all sm:my-8 w-full sm:max-w-lg bg-white drop-shadow-1 dark:bg-boxdark dark:drop-shadow-none"
                    >
                        {children}
                    </div>
                </div>
            </div>
        </div>
    );
}
