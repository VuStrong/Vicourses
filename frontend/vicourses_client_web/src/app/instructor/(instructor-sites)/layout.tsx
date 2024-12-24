import { Metadata } from "next";
import InstructorSideBar from "./_components/InstructorSideBar";
import InstructorPagesHeader from "./_components/InstructorPagesHeader";

export const metadata: Metadata = {
    title: "Instructor | Vicourses",
    openGraph: {
        title: "Instructor | Vicourses",
    },
};

export default function InstructorPagesLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <div className="flex">
            <div className="lg:block hidden">
                <InstructorSideBar />
            </div>
            <div className="flex-1">
                <InstructorPagesHeader />
                <div className="px-5">
                    {children}
                </div>
            </div>
        </div>
    );
}
