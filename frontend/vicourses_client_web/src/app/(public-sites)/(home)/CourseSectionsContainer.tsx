import Link from "next/link";
import { getCategories } from "@/services/api/category";
import { getCourses } from "@/services/api/course";
import CoursesCarousel from "./_components/CoursesCarousel";

export default async function CourseSectionsContainer() {
    const categoryIds = (await getCategories())
        .filter((c) => c.parentId !== null)
        .slice(0, 4)
        .map((c) => c.id);

    const tasks = categoryIds.map((id) => getCourses({ subCategoryId: id }));
    const results = await Promise.allSettled(tasks);

    return (
        <div>
            {results.map((result, index) => {
                if (result.status === "rejected" || !result.value?.items[0])
                    return null;

                const category = result.value.items[0].subCategory;

                return (
                    <div key={index} className="mb-10">
                        <h4 className="text-black font-bold text-xl mb-3 flex gap-1 items-center">
                            <Link
                                href={`/category/${category.slug}`}
                                className="text-primary underline"
                            >
                                {category.name}
                            </Link>
                            courses
                        </h4>
                        <section>
                            <CoursesCarousel courses={result.value.items} />
                        </section>
                    </div>
                );
            })}
        </div>
    );
}
