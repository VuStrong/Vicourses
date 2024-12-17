import { Suspense } from "react";
import { Loader } from "@/components/common";
import CategoriesSection from "./CategoriesSection";
import CourseSectionsContainer from "./CourseSectionsContainer";
import UserCoursesSection from "./UserCoursesSection";

export default function HomePage() {
    return (
        <>
            <UserCoursesSection />

            <div className="mt-5">
                <h2 className="text-black font-bold text-2xl mb-3">
                    Categories
                </h2>

                <Suspense
                    fallback={
                        <div className="flex justify-center">
                            <Loader />
                        </div>
                    }
                >
                    <CategoriesSection />
                </Suspense>
            </div>

            <div className="mt-10 mb-20">
                <h2 className="text-black font-bold text-2xl mb-3">
                    Discovery
                </h2>

                <Suspense
                    fallback={
                        <div className="flex justify-center">
                            <Loader />
                        </div>
                    }
                >
                    <CourseSectionsContainer />
                </Suspense>
            </div>
        </>
    );
}
