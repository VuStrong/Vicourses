import { Metadata } from "next";
import Tabs from "./_components/Tabs";
import PublicSitesHeader from "@/components/headers/PublicSitesHeader";

export const metadata: Metadata = {
    title: "My courses | Vicourses",
    openGraph: {
        title: "My courses | Vicourses",
    },
};

export default function MyCoursesPagesLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <>
            <PublicSitesHeader />

            <div className="bg-[#1c1d1f]">
                <div className="container mx-auto px-3 sm:px-0 lg:max-w-[60%] pt-10">
                    <h1 className="text-white text-3xl font-bold mb-5">
                        Learning
                    </h1>
                    <Tabs />
                </div>
            </div>

            <div className="container mx-auto px-3 sm:px-0 lg:max-w-[60%] my-10">
                {children}
            </div>
        </>
    );
}
