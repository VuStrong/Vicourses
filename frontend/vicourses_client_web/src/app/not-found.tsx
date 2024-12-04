export default function NotFound() {
    return (
        <>
            <section className="grid h-screen pt-32 pb-16">
                <div className="container grid content-center gap-12 lg:max-w-5xl lg:grid-cols-2 lg:items-center">
                    <div className="justify-self-center text-center lg:text-left">
                        <h1 className="pb-4 text-5xl font-bold lg:text-6xl text-error">
                            Error 404
                        </h1>
                        <p className="pb-8 font-semibold">
                            We couldn't find the page you were looking for
                        </p>
                        <a
                            href="/"
                            className="inline-flex items-center justify-center rounded-full bg-primary py-4 px-8 font-bold text-black"
                        >
                            Go to home
                        </a>
                    </div>

                    <div className="justify-self-center">
                        <img
                            src="/img/logo.png"
                            className="w-64 lg:w-[400px]"
                            alt="Strongtify"
                        />
                        <div className="mx-auto h-8 w-36 animate-shadow rounded-[50%] bg-gray-900/30 blur-md lg:w-64"></div>
                    </div>
                </div>

                <div className="flex items-center justify-center gap-2 self-end text-sm font-semibold">
                    <p>vicoursesvn@gmail.com</p>
                </div>
            </section>
        </>
    );
}