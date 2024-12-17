import Link from "next/link";
import { getCategories } from "@/services/api/category";

export default async function CategoriesSection() {
    const categories = await getCategories();

    return (
        <section
            className="grid grid-flow-col overflow-x-scroll gap-2 md:gap-5 text-black font-semibold"
            style={{
                gridTemplateRows: "auto auto",
            }}
        >
            {categories.map((category) => (
                <Link
                    key={category.id}
                    href={`/category/${category.slug}`}
                    className="px-5 py-3 text-center whitespace-nowrap border border-gray-300 hover:bg-primary hover:text-white"
                >
                    {category.name}
                </Link>
            ))}
        </section>
    );
}
