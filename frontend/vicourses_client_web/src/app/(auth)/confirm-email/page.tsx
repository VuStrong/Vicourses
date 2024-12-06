import type { Metadata } from "next";
import Link from "next/link";
import { redirect } from "next/navigation";

import { confirmEmail } from "@/services/api/auth";
import { auth } from "@/libs/auth";
import { getAuthenticatedUser } from "@/services/api/user";

export const metadata: Metadata = {
    title: "Confirm email",
};

export default async function ConfirmEmailPage({
    searchParams,
}: {
    searchParams?: { [key: string]: string | undefined };
}) {
    const session = await auth();
    const user = session
        ? await getAuthenticatedUser(session.accessToken, "id,emailConfirmed")
        : null;

    if (user && user.emailConfirmed) {
        redirect("/");
    }

    let isSuccess = false;
    if (searchParams?.userId && searchParams?.token) {
        try {
            await confirmEmail(searchParams.userId, searchParams.token);
            isSuccess = true;
        } catch (error) {}
    }

    return (
        <section className="lg:h-auto md:h-auto border-0 rounded-lg shadow-2xl flex flex-col w-full bg-white outline-none focus:outline-none">
            <div className="relative p-6 flex-auto">
                <div className="flex flex-col gap-4">
                    <div className="text-center text-gray-900">
                        {isSuccess ? (
                            <>
                                <div className="text-xl md:text-2xl font-bold text-success mb-2">
                                    Your account has been verified.
                                </div>
                                <br />
                                {user ? (
                                    <Link href="/" className="underline">
                                        Go to home page
                                    </Link>
                                ) : (
                                    <Link href="/login" className="underline">
                                        Go to Sign in page
                                    </Link>
                                )}
                            </>
                        ) : (
                            <>
                                <div className="text-xl md:text-2xl font-bold text-error mb-2">
                                    Account verification failed!
                                </div>
                                <p>We cannot verify your account</p>
                            </>
                        )}
                    </div>
                </div>
            </div>
        </section>
    );
}
