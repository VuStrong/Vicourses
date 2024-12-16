"use client";

import { usePathname, useRouter } from "next/navigation";
import { Button } from "@material-tailwind/react";
import { GrPrevious, GrNext } from "react-icons/gr";
import { IoMenu } from "react-icons/io5";

import useSidebar from "../hooks/useSidebar";
import useLessonNavigation from "../hooks/useLessonNavigation";

export default function BottomNav() {
    const toggleSidebar = useSidebar((state) => state.toggle);
    const prevLessonId = useLessonNavigation(state => state.prevId);
    const nextLessonId = useLessonNavigation(state => state.nextId);
    const router = useRouter();
    const pathname = usePathname();

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
                    disabled={!prevLessonId}
                    onClick={() => {
                        router.push(`${pathname}?lesson=${prevLessonId}`);
                    }}
                    className={`bg-white rounded-full px-5 text-primary border border-primary flex gap-2 items-center`}
                >
                    <GrPrevious size={16} />
                    <span className="md:block hidden">
                        Prev lesson
                    </span>
                </Button>
                <Button
                    disabled={!nextLessonId}
                    onClick={() => {
                        router.push(`${pathname}?lesson=${nextLessonId}`);
                    }}
                    className={`bg-primary rounded-full px-5 text-white flex gap-2 items-center`}
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
