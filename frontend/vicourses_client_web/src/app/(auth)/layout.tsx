import Image from "next/image";
import Link from "next/link";

export default async function AuthLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <section>
            {/* Header */}
            <header className="bg-white py-2 px-5">
                <Link href="/" className="w-[60px] block">
                    <Image
                        src="/img/logo-transparent.png"
                        width={100}
                        height={50}
                        alt="Vicourses"
                    />
                </Link>
            </header>

            <div className="text-yellow-50 justify-center flex inset-0 z-50 min-h-screen">
                <div className="relative w-11/12 md:w-4/6 lg:w-3/6 xl:w-2/5 my-6 mx-auto h-5/6 lg:h-auto md:h-auto">
                    <div className="h-full">{children}</div>
                </div>
            </div>
        </section>
    );
}
