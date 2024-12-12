"use client";

import {
    Card,
    List,
    ListItem,
    ListItemPrefix,
    Typography,
} from "@material-tailwind/react";
import Link from "next/link";
import Image from "next/image";
import { MdOutlineOndemandVideo, MdOutlineBarChart } from "react-icons/md";
import { IoIosStar } from "react-icons/io";

export default function InstructorSideBar() {
    return (
        <Card className="min-h-screen h-full w-full max-w-[20rem] p-4 shadow-xl shadow-blue-gray-900/5 bg-[#1c1d1f] rounded-none">
            <div className="mb-2 p-4">
                <Typography variant="h5" color="blue-gray">
                    <Link href="/" className="w-[60px] block">
                        <Image
                            src="/img/logo-transparent.png"
                            width={100}
                            height={50}
                            alt="Vicourses"
                        />
                    </Link>
                </Typography>
            </div>
            <List className="text-gray-50">
                <Link href="/instructor/courses">
                    <ListItem>
                        <ListItemPrefix>
                            <MdOutlineOndemandVideo className="h-5 w-5" />
                        </ListItemPrefix>
                        Courses
                    </ListItem>
                </Link>
                <Link href="/instructor/performance">
                    <ListItem>
                        <ListItemPrefix>
                            <MdOutlineBarChart className="h-5 w-5" />
                        </ListItemPrefix>
                        Performance
                    </ListItem>
                </Link>
                <Link href="/instructor/ratings">
                    <ListItem>
                        <ListItemPrefix>
                            <IoIosStar className="h-5 w-5" />
                        </ListItemPrefix>
                        Ratings
                    </ListItem>
                </Link>
            </List>
        </Card>
    );
}
