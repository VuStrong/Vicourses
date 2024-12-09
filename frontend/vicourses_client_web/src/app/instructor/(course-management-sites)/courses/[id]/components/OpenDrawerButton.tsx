"use client";

import { Drawer } from "@material-tailwind/react";
import { useState } from "react";
import { MdMenu } from "react-icons/md";
import CourseManagementSidebar from "./CourseManagementSidebar";

export default function OpenDrawerButton({ courseId }: { courseId: string }) {
    const [open, setOpen] = useState<boolean>(false);

    return (
        <>
            <Drawer
                open={open}
                onClose={() => setOpen(false)}
                className="overflow-y-auto"
                overlayProps={{
                    className: "fixed",
                }}
            >
                <CourseManagementSidebar
                    courseId={courseId}
                    onCLickItem={() => setOpen(false)}
                />
            </Drawer>

            <button
                type="button"
                className="p-2 rounded-lg focus:outline-none focus:ring-2"
                onClick={() => setOpen(true)}
            >
                <MdMenu size={28} className="text-gray-900" />
            </button>
        </>
    );
}
