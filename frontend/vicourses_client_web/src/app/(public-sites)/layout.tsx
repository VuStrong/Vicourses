import PublicSitesHeader from "@/app/(public-sites)/components/PublicSitesHeader";

export default async function PublicSitesLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <>
            <PublicSitesHeader />

            <div className="container mx-auto">
                {children}
            </div>
        </>
    );
}
