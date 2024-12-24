import type { Metadata } from "next";
import RegisterForm from "./_components/RegisterForm";

export const metadata: Metadata = {
    title: "Register new account",
};

export default async function RegisterPage() {
    return <RegisterForm />;
}