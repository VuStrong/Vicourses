"use client";

import {
    Button,
    Card,
    List,
    ListItem,
    ListItemPrefix,
    Typography,
} from "@material-tailwind/react";
import Link from "next/link";
import Image from "next/image";
import { CgProfile } from "react-icons/cg";
import { AiOutlineProfile } from "react-icons/ai";
import { MdPrivacyTip } from "react-icons/md";
import { FaPaypal } from "react-icons/fa";
import { MdOutlineSecurity } from "react-icons/md";
import BecomeInstructorButton from "./BecomeInstructorButton";

export default function ProfileSideBar() {
    return (
        <Card className="min-h-screen w-full max-w-[20rem] p-4 shadow-xl shadow-blue-gray-900/5 bg-[#1c1d1f] rounded-none">
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
                <Link href="/profile">
                    <ListItem>
                        <ListItemPrefix>
                            <AiOutlineProfile className="h-5 w-5" />
                        </ListItemPrefix>
                        Profile
                    </ListItem>
                </Link>
                <Link href="/profile/photo">
                    <ListItem>
                        <ListItemPrefix>
                            <CgProfile className="h-5 w-5" />
                        </ListItemPrefix>
                        Profile photo
                    </ListItem>
                </Link>
                <Link href="/profile/privacy">
                    <ListItem>
                        <ListItemPrefix>
                            <MdPrivacyTip className="h-5 w-5" />
                        </ListItemPrefix>
                        Privary
                    </ListItem>
                </Link>
                <Link href="/profile/payouts">
                    <ListItem>
                        <ListItemPrefix>
                            <FaPaypal className="h-5 w-5" />
                        </ListItemPrefix>
                        Payouts
                    </ListItem>
                </Link>
                <Link href="/profile/security">
                    <ListItem>
                        <ListItemPrefix>
                            <MdOutlineSecurity className="h-5 w-5" />
                        </ListItemPrefix>
                        Security
                    </ListItem>
                </Link>
            </List>

            <BecomeInstructorButton />
        </Card>
    );
}
