import { Metadata } from "next";
import ProfileHeader from "./components/ProfileHeader";
import ProfileSideBar from "./components/ProfileSideBar";

export const metadata: Metadata = {
    title: "Profile",
};

export default function ProfilePagesLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <div className="flex">
            <div className="lg:block hidden">
                <ProfileSideBar />
            </div>
            <div className="lg:flex-grow w-full">
                <ProfileHeader />
                <div className="px-5">
                    {children}
                </div>
            </div>
        </div>
    )
}