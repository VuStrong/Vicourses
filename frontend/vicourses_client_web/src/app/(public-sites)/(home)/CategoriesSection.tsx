"use client"

import { useEffect, useState } from "react";
import Link from "next/link";
import { getCategories } from "@/services/api/category";
import { Category } from "@/libs/types/category";
import { Loader } from "@/components/common";

export default function CategoriesSection() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [categories, setCategories] = useState<Category[]>([]);

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const result = await getCategories();

            setCategories(result);
            setIsLoading(false);
        })();
    }, []);

    return (
        <section className="mb-10">
            <h2 className="text-black font-bold text-2xl mb-3">Categories</h2>

            {isLoading && (
                <div className="flex justify-center"><Loader /></div>
            )}

            {!isLoading && (
                <div
                    style={{
                        gridTemplateRows: "auto auto",
                    }}
                    className="grid grid-flow-col overflow-x-scroll gap-2 md:gap-5 text-black font-semibold no-scrollbar"
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
                </div>
            )}
        </section>
    );
}
