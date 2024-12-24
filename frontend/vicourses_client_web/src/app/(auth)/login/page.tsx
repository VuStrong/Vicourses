import type { Metadata } from "next";
import LoginForm from "./_components/LoginForm";

export const metadata: Metadata = {
    title: "Sign in to Vicourses",
};

export default async function LoginPage() {
    return <LoginForm />;
}