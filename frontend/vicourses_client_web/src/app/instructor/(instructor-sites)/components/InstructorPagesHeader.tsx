"use client"

import { useState } from "react";
import { MdOutlineMenu } from "react-icons/md";
import { Drawer } from "@material-tailwind/react";
import UserMenu from "@/components/common/UserMenu";
import InstructorSideBar from "./InstructorSideBar";

export default function InstructorPagesHeader() {
    const [isDrawerOpen, setIsDrawerOpen] = useState<boolean>(false);
    const openDrawer = () => setIsDrawerOpen(true);
    const closeDrawer = () => setIsDrawerOpen(false);

    return (
        <header className="flex justify-between lg:justify-end items-center py-2 px-5 bg-transparent">
            <Drawer
                open={isDrawerOpen}
                onClose={closeDrawer}
                overlayProps={{
                    className: "fixed",
                }}
            >
                <InstructorSideBar />
            </Drawer>

            <div className="lg:hidden block">
                <button
                    type="button"
                    onClick={openDrawer}
                    className="p-2 rounded-lg focus:outline-none focus:ring-2"
                >
                    <MdOutlineMenu size={24} className="text-gray-900" />
                </button>
            </div>
            <UserMenu />
        </header>
    );
}