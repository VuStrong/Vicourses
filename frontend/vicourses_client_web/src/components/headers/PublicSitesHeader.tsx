import Link from "next/link";
import Image from "next/image";
import UserMenu from "./UserMenu";

export default function PublicSitesHeader() {
    return (
        <header className={`flex justify-between items-center py-2 px-5 bg-white`}>
            <Link href="/" className="w-[60px]">
                <Image
                    src="/img/logo-transparent.png"
                    width={100}
                    height={50}
                    alt="Vicourses"
                />
            </Link>

            <UserMenu />
        </header>
    );
}
