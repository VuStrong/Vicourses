import { Suspense } from "react";
import CategoriesSection from "./CategoriesSection";
import { Loader } from "@/components/common";

export default function HomePage() {
    return (
        <div className="p-5">
            <Suspense
                fallback={
                    <div className="flex justify-center items-center min-h-32">
                        <Loader />
                    </div>
                }
            >
                <CategoriesSection />
            </Suspense>

            <div>Home</div>
        </div>
    );
}
