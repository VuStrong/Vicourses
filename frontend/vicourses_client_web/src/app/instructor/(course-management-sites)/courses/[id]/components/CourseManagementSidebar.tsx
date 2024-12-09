"use client";

import { Card, List, ListItem } from "@material-tailwind/react";
import Link from "next/link";
import { usePathname } from "next/navigation";

export default function CourseManagementSidebar({
    courseId,
    onCLickItem,
}: {
    courseId: string;
    onCLickItem?: () => void;
}) {
    const pathname = usePathname();

    return (
        <Card className="min-h-screen h-full w-full max-w-[20rem] p-4 bg-white shadow-none rounded-none border-none">
            <List className="text-gray-800">
                <div className="text-black font-bold p-3 text-lg">Basics</div>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${courseId}`}
                    className={`${pathname.endsWith(courseId) && "border-l-4 border-gray-800"}`}
                >
                    <ListItem>Overview</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${courseId}/goals`}
                    className={`${pathname.endsWith("goals") && "border-l-4 border-gray-800"}`}
                >
                    <ListItem>Goals</ListItem>
                </Link>
                <Link
                    onClick={onCLickItem}
                    href={`/instructor/courses/${courseId}/settings`}
                    className={`${pathname.endsWith("settings") && "border-l-4 border-gray-800"}`}
                >
                    <ListItem>Settings</ListItem>
                </Link>
            </List>
        </Card>
    );
}
