"use client";

import useSidebar from "../hooks/useSidebar";
import Curriculum from "./Curriculum";

export default function Sidebar({
    courseId,
    lessonId,
}: {
    courseId: string;
    lessonId?: string;
}) {
    const isOpen = useSidebar((state) => state.isOpen);
    const setIsOpen = useSidebar((state) => state.setIsOpen);

    return (
        <>
            <div
                className={`lg:!hidden fixed inset-0 bg-black/70 z-[1002] ${
                    isOpen ? "block" : "hidden"
                }`}
                onClick={() => setIsOpen(false)}
            ></div>
            <aside className={`fixed left-auto lg:!translate-x-0 lg:right-0 top-0 h-screen w-[80%] md:w-96 bg-white border-l border-gray-700 
                transition-transform z-[1003] lg:z-[999] overflow-y-scroll pb-20 ${isOpen ? "translate-x-0" : "-translate-x-full"}`}>
                <Curriculum courseId={courseId} lessonId={lessonId} />
            </aside>
        </>
    );
}
