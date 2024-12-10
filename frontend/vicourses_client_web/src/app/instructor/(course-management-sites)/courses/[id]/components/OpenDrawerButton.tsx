"use client";

import { Drawer } from "@material-tailwind/react";
import { useState } from "react";
import { MdMenu } from "react-icons/md";
import CourseManagementSidebar from "./CourseManagementSidebar";
import { Course } from "@/libs/types/course";

export default function OpenDrawerButton({ course }: { course: Course }) {
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
                    course={course}
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
