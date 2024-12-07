import { Metadata } from "next";
import { notFound } from "next/navigation";
import { getCategoryBySlug } from "@/services/api/category";
import CoursesContainer from "./CoursesContainer";

export async function generateMetadata({
    params,
}: {
    params: { slug: string };
}): Promise<Metadata> {
    const category = await getCategoryBySlug(params.slug);

    if (!category) notFound();

    return {
        title: `Online ${category.name} Courses`,
        description: `Online ${category.name} Courses`,
        openGraph: {
            title: `Online ${category.name} Courses`,
            description: `Online ${category.name} Courses`,
        },
    };
}

export default async function CategoryPage({
    params,
}: {
    params: { slug: string };
}) {
    const category = await getCategoryBySlug(params.slug);

    if (!category) notFound();

    return (
        <div>
            <h1 className="text-black font-bold text-2xl mt-5">
                {category.name} Courses
            </h1>

            <CoursesContainer category={category} />
        </div>
    );
}
