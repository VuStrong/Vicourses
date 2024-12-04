import type { Metadata } from "next";
import { auth } from "@/libs/auth";
import { redirect } from "next/navigation";
import LoginForm from "./LoginForm";

export const metadata: Metadata = {
    title: "Sign in to Vicourses",
};

export default async function LoginPage() {
    const session = await auth();

    // if user is already loged in, redirect to home page
    if (session) {
        redirect("/");
    }
    
    return <LoginForm />;
}