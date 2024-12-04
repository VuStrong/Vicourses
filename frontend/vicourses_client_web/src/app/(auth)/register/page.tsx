import type { Metadata } from "next";
import { auth } from "@/libs/auth";
import { redirect } from "next/navigation";
import RegisterForm from "./RegisterForm";

export const metadata: Metadata = {
    title: "Register new account",
};

export default async function RegisterPage() {
    const session = await auth();

    // if user is already loged in, redirect to home page
    if (session) {
        redirect("/");
    }
    
    return <RegisterForm />;
}