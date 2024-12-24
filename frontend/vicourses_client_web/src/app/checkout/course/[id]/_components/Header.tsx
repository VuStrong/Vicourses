import Link from "next/link";
import Image from "next/image";

export default function Header() {
    return (
        <header
            className={`flex items-center justify-between py-2 px-5 bg-white shadow-lg`}
        >
            <Link href="/" className="w-[60px]">
                <Image
                    src="/img/logo-transparent.png"
                    width={100}
                    height={50}
                    alt="Vicourses"
                />
            </Link>
        </header>
    );
}
