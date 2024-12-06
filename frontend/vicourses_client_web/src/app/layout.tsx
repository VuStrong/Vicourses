import type { Metadata } from "next";
import localFont from "next/font/local";
import "./globals.css";
import Footer from "@/components/Footer";
import ToasterProvider from "@/providers/ToasterProvider";
import SessionProviderWrapper from "@/providers/SessionProvider";

const geistSans = localFont({
    src: "./fonts/GeistVF.woff",
    variable: "--font-geist-sans",
    weight: "100 900",
});
const geistMono = localFont({
    src: "./fonts/GeistMonoVF.woff",
    variable: "--font-geist-mono",
    weight: "100 900",
});

export const metadata: Metadata = {
    title: "Vicourses",
    description:
        "Vicourses is an online learning and teaching marketplace with over 250000 courses and 73 million students. Learn programming, marketing, data science and more.",
	openGraph: {
        title: "Vicourses",
        description:
	        "Vicourses is an online learning and teaching marketplace with over 250000 courses and 73 million students. Learn programming, marketing, data science and more.",
        type: "website",
    },
};

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en">
			<head>
				<link rel="icon" type="image/png" href="/favicon-48x48.png" sizes="48x48" />
				<link rel="icon" type="image/svg+xml" href="/favicon.svg" />
				<link rel="shortcut icon" href="/favicon.ico" />
				<link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon.png" />
				<link rel="manifest" href="/site.webmanifest" />
			</head>
            <body
                className={`${geistSans.variable} ${geistMono.variable} antialiased`}
            >
				<ToasterProvider />

				<SessionProviderWrapper>
					<div className="min-h-screen">
                		{children}
					</div>
				</SessionProviderWrapper>
				
				<Footer />
            </body>
        </html>
    );
}
