import { Metadata } from "next";
import CoursesContainer from "./_components/CoursesContainer";

export const metadata: Metadata = {
    title: "Search | Vicourses",
    openGraph: {
        title: "Search | Vicourses",
    },
};

export default function SearchPage({
    searchParams,
}: {
    searchParams?: { [key: string]: string | string[] | undefined };
}) {
    const keyword = searchParams?.q
        ? Array.isArray(searchParams.q)
            ? searchParams.q[0]
            : searchParams.q
        : undefined;

    return (
        <div>
            <h1 className="text-black font-bold text-2xl mt-5">
                Results for "{keyword}"
            </h1>

            <CoursesContainer keyword={keyword} />
        </div>
    );
}
