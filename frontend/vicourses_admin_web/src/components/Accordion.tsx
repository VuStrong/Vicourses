import { useState } from "react";

export default function Accordion({
    title,
    defaultOpen,
    info,
    children,
}: {
    title: string;
    defaultOpen?: boolean;
    info: React.ReactNode;
    children: React.ReactNode;
}) {
    const [open, setOpen] = useState<boolean>(defaultOpen || false);

    return (
        <>
            <div
                onClick={() => setOpen(!open)}
                className={`border-x border-t last:border-b px-5 py-3 flex items-center justify-between cursor-pointer dark:border-gray-600 bg-[#f7f9fa] dark:bg-boxdark-2`}
            >
                <div className="flex gap-3">
                    {open ? <div>&#11205;</div> : <div>&#11206;</div>}
                    <div className="font-bold line-clamp-1">{title}</div>
                </div>
                <div className="hidden md:block whitespace-nowrap">{info}</div>
            </div>
            {open && (
                <div className="border-x border-t dark:border-gray-600 last:border-b p-5">
                    {children}
                </div>
            )}
        </>
    );
}
