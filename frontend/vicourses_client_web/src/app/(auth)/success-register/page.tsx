import type { Metadata } from "next";
import { redirect } from "next/navigation";
import { auth } from "@/libs/auth";
import { getAuthenticatedUser } from "@/services/api/user";
import SendEmailConfirmationButton from "./_components/SendEmailConfirmationButton";
import SignOutButton from "./_components/SignOutButton";

export const metadata: Metadata = {
    title: "Success register",
};

export default async function SuccessRegisterPage() {
    const session = await auth();
    if (!session) {
        redirect("/login");
    }

    const user = await getAuthenticatedUser(session.accessToken, "email,emailConfirmed");

    if (!user || user.emailConfirmed) {
        redirect("/");
    }

    return (
        <section className="lg:h-auto md:h-auto border-0 rounded-lg shadow-2xl flex flex-col w-full bg-white outline-none focus:outline-none">
            <div className="relative p-6 flex-auto">
                <div className="flex flex-col gap-4">
                    <div className="text-center text-gray-900 flex flex-col items-center justify-center">
                        <div className="text-xl md:text-2xl font-bold text-success mb-2">
                            Your account has been created but has not been verified.
                        </div>
                        <p>
                            We've sent you an email to verify your account, please check your email.
                        </p>
                        <br />
                        <div>
                            Don't get email?
                        </div>
                        <div className="mb-3">
                            <SendEmailConfirmationButton
                                email={user.email}
                            />
                        </div>
                        <SignOutButton />
                    </div>
                </div>
            </div>
        </section>
    );
}