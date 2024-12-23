import { Suspense } from "react";
import { Loader } from "@/components/common";
import CategoriesSection from "./CategoriesSection";
import CourseSectionsContainer from "./CourseSectionsContainer";
import UserCoursesSection from "./UserCoursesSection";

export default function HomePage() {
    return (
        <>
            <div className="mt-5">
                <div className="relative">
                    <div className="w-full max-h-96 aspect-video">
                        <img
                            src="https://res.cloudinary.com/dsrcm9jcs/image/upload/v1734940749/Others/banner1.jpg"
                            alt="banner"
                            className="w-full h-full object-cover"
                        />
                    </div>
                    <div className="md:absolute bg-white p-5 left-10 top-10 md:shadow-xl md:max-w-[23rem]">
                        <h1 className="text-black text-2xl font-bold mb-3">
                            Study what you are interested in
                        </h1>
                        <div className="text-gray-800 text-lg">
                            Skills for your present (and your future). Start
                            studying with us.
                        </div>
                    </div>
                </div>
            </div>

            <UserCoursesSection />

            <div className="mt-10">
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
