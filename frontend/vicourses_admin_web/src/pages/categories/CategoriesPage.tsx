import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaPen } from "react-icons/fa";

import { Category } from "../../types/category";
import axiosInstance from "../../libs/axios";
import SearchBar from "../../components/SearchBar";
import Loader from "../../components/Loader";

export default function CategoriesPage() {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>([]);
    const [search, setSearch] = useState<string>();

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const result = await axiosInstance.get<Category[]>(
                "/api/cs/v1/categories",
                { params: { q: search } },
            );

            setCategories(result.data);
            setIsLoading(false);
        })();
    }, [search]);

    return (
        <div>
            <h1 className="text-2xl font-bold mb-5">Categories</h1>

            <div className="max-w-60 mb-5">
                <SearchBar
                    placeholder="Search categories"
                    onSubmit={(value) => setSearch(value)}
                />
            </div>

            <div>
                {isLoading ? (
                    <div className="flex justify-center"><Loader /></div>
                ) : (
                    <CategoriesTable categories={categories} />
                )}
            </div>
        </div>
    );
}

function CategoriesTable({
    categories,
}: {
    categories: Category[];
}) {
    return (
        <div className="rounded-sm border border-stroke bg-white px-5 pt-6 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
            <div className="flex flex-col">
                <div className="grid grid-cols-3 rounded-sm bg-gray-2 dark:bg-meta-4 sm:grid-cols-5">
                    <div className="p-2.5 xl:p-5">
                        <h5 className="text-sm font-medium uppercase xsm:text-base">
                            Name
                        </h5>
                    </div>
                    <div className="p-2.5 xl:p-5">
                        <h5 className="text-sm font-medium uppercase xsm:text-base">
                            Slug
                        </h5>
                    </div>
                    <div className="p-2.5 xl:p-5">
                        <h5 className="text-sm font-medium uppercase xsm:text-base">
                            Updated At
                        </h5>
                    </div>
                    <div className="p-2.5 xl:p-5">
                        <h5 className="text-sm font-medium uppercase xsm:text-base">
                            Actions
                        </h5>
                    </div>
                </div>

                {categories.map((category, key) => (
                    <div
                        className={`grid grid-cols-3 sm:grid-cols-5 ${
                            key === categories.length - 1
                                ? ""
                                : "border-b border-stroke dark:border-strokedark"
                        }`}
                        key={key}
                    >
                        <div className="flex items-center gap-3 p-2.5 xl:p-5">
                            <p className="hidden text-black dark:text-white sm:block">
                                {category.name}
                            </p>
                        </div>

                        <div className="flex items-center p-2.5 xl:p-5">
                            <p className="text-black dark:text-white">
                                {category.slug}
                            </p>
                        </div>

                        <div className="flex items-center p-2.5 xl:p-5">
                            <p className="text-black dark:text-white">
                                {new Date(category.updatedAt).toLocaleDateString()}
                            </p>
                        </div>
                        <div className="flex items-center p-2.5 xl:p-5">
                            <Link to={`/categories/${category.slug}`}>
                                <FaPen />
                            </Link>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};
