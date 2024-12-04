import type { Metadata } from "next";
import { auth } from "@/libs/auth";
import { redirect } from "next/navigation";
import ChooseCategoriesForm from "./ChooseCategoriesForm";

export const metadata: Metadata = {
    title: "Choose your favorite categories",
};

export default async function ChooseCategoriesPage() {
    const session = await auth();

    if (!session) {
        redirect("/login");
    }
    
    return <ChooseCategoriesForm />;
}