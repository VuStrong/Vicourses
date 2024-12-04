import type { Metadata } from "next";
import ResetPasswordForm from "./ResetPasswordForm";

export const metadata: Metadata = {
    title: "Reset password",
};

export default function ResetPasswordPage({
    searchParams,
}: {
    searchParams?: { [key: string]: string | undefined };
}) {
    return (
        <ResetPasswordForm
            userId={searchParams?.userId ?? ""}
            token={searchParams?.token ?? ""}
        />
    );
}
