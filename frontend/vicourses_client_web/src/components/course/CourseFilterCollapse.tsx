"use client";

import { useState } from "react";

export default function CourseFilterCollapse({
    filterName,
    children,
}: {
    filterName: string;
    children: React.ReactNode;
}) {
    const [isOpen, setIsOpen] = useState<boolean>(false);

    return (
        <div className="border-t-4">
            <div
                className="flex items-center justify-between py-3 cursor-pointer"
                onClick={() => setIsOpen(!isOpen)}
            >
                <div className="text-black font-bold text-lg">{filterName}</div>
                {isOpen ? <div>&#11205;</div> : <div>&#11206;</div>}
            </div>
            {isOpen && children}
        </div>
    );
}
