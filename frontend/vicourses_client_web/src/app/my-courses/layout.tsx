import { Metadata } from "next";
import Tabs from "./Tabs";
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
        
            <div className="container mx-auto px-3 sm:px-0 mt-5 lg:max-w-[60%]">
                <h1 className="text-black text-3xl font-bold mb-5">Learning</h1>
                <Tabs />

                <div className="my-10">
                    {children}
                </div>
            </div>
        </>
    )
}