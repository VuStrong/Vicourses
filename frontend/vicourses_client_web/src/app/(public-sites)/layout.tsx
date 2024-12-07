import PublicSitesHeader from "@/components/headers/PublicSitesHeader";

export default async function PublicSitesLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <>
            <PublicSitesHeader />

            <div className="container mx-auto px-3 sm:px-0">
                {children}
            </div>
        </>
    );
}
