import type { Metadata } from "next";
import ChooseCategoriesForm from "./ChooseCategoriesForm";

export const metadata: Metadata = {
    title: "Choose your favorite categories",
};

export default async function ChooseCategoriesPage() {
    return <ChooseCategoriesForm />;
}