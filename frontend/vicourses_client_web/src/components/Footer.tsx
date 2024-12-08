import Link from "next/link";

export default function Footer() {
    return (
        <footer className="pt-10 px-5 pb-28 bg-[#1c1d1f]">
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-5 mb-10">
                <div>
                    <p className="text-yellow-50 text-lg font-medium mb-2">
                        Info
                    </p>
                    <ul className="text-gray-400 flex flex-col gap-2 list-none m-0 p-0">
                        <li>
                            <a
                                href={`${process.env.NEXT_PUBLIC_BACKEND_URL}/swagger`}
                                className="hover:text-primary"
                                target="_blank"
                            >
                                Vicourses API
                            </a>
                        </li>
                        <li>
                            <Link
                                href="/privacy"
                                className="hover:text-primary"
                            >
                                Privacy
                            </Link>
                        </li>
                        <li>
                            <Link
                                href="/license"
                                className="hover:text-primary"
                            >
                                Copyright
                            </Link>
                        </li>
                    </ul>
                </div>

                <div>
                    <p className="text-yellow-50 text-lg font-medium mb-2">
                        Sites
                    </p>
                    <ul className="text-gray-400 flex flex-col gap-2 list-none m-0 p-0">
                        <li>
                            <a
                                href="https://strongtify.io.vn"
                                className="hover:text-primary"
                                target="_blank"
                            >
                                Strongtify
                            </a>
                        </li>
                        <li>
                            <a
                                href="https://github.com/VuStrong"
                                className="hover:text-primary"
                                target="_blank"
                            >
                                Github
                            </a>
                        </li>
                    </ul>
                </div>

                <div>
                    <p className="text-yellow-50 text-lg font-medium mb-2">
                        Helps
                    </p>
                    <ul className="text-gray-400 flex flex-col gap-2 list-none m-0 p-0">
                        <li>
                            <Link
                                href="/contact"
                                className="hover:text-primary"
                            >
                                Contact
                            </Link>
                        </li>
                        <li>
                            <a href="#" className="hover:text-primary">
                                Questions
                            </a>
                        </li>
                        <li>
                            <a href="#" className="hover:text-primary">
                                News
                            </a>
                        </li>
                    </ul>
                </div>
            </div>

            <div>
                <p className="text-gray-400">&copy; 2024 - Vicourses</p>
            </div>
        </footer>
    );
}