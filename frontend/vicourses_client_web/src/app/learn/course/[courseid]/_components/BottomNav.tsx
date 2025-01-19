"use client";

import { Button } from "@material-tailwind/react";
import { GrPrevious, GrNext } from "react-icons/gr";
import { IoMenu } from "react-icons/io5";

import useSidebar from "../_hooks/useSidebar";
import useCurriculum from "../_hooks/useCurriculum";

export default function BottomNav() {
    const toggleSidebar = useSidebar((state) => state.toggle);
    const prevLesson = useCurriculum(state => state.prevLesson);
    const nextLesson = useCurriculum(state => state.nextLesson);
    const setCurrentLesson = useCurriculum(state => state.setCurrentLesson);

    return (
        <div className="fixed bottom-0 left-0 w-full flex items-center justify-between p-2 bg-[#f0f0f0] z-[1000] border-t border-gray-600">
            <div>
                <button
                    onClick={toggleSidebar}
                    className="bg-white rounded-full p-2 block lg:hidden"
                >
                    <IoMenu size={24} />
                </button>
            </div>
            <div className="flex gap-2">
                <Button
                    disabled={!prevLesson}
                    onClick={() => {
                        setCurrentLesson(prevLesson!.id);
                    }}
                    className={`bg-white rounded-full px-5 text-primary border border-primary flex gap-2 items-center`}
                    title={`${prevLesson?.title || ""}`}
                >
                    <GrPrevious size={16} />
                    <span className="md:block hidden">
                        Prev lesson
                    </span>
                </Button>
                <Button
                    disabled={!nextLesson}
                    onClick={() => {
                        setCurrentLesson(nextLesson!.id);
                    }}
                    className={`bg-primary rounded-full px-5 text-white flex gap-2 items-center`}
                    title={`${nextLesson?.title || ""}`}
                >
                    <span className="md:block hidden">
                        Next lesson
                    </span>
                    <GrNext size={16} />
                </Button>
            </div>
            <div className="lg:block hidden"></div>
        </div>
    );
}
