import PublicSitesHeader from "@/components/headers/PublicSitesHeader";
import { auth } from "@/libs/auth";
import { getAuthenticatedUser } from "@/services/api/user";
import { redirect } from "next/navigation";

export default async function PublicSitesLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    const session = await auth();
    const user = session
        ? await getAuthenticatedUser(session.accessToken, "emailConfirmed")
        : null;

    if (user && !user.emailConfirmed) {
        redirect("/success-register");
    }

    return (
        <>
            <PublicSitesHeader />

            {children}
        </>
    );
}
