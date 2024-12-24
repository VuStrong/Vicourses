import type { Metadata } from "next";
import ForgotPasswordForm from "./_components/ForgotPasswordForm";

export const metadata: Metadata = {
    title: "Forgot password",
};

export default function ForgotPasswordPage() {
    return <ForgotPasswordForm />;
}
