import Header from "./Header";

export default function CheckoutPageLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <>
            <Header />
            <div className="container mx-auto px-3 sm:px-0 mt-10">
                {children}
            </div>
        </>
    )
}