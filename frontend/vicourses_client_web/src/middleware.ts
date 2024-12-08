import { auth } from "@/libs/auth";
import { getAuthenticatedUser, linkPaypalAccount } from "./services/api/user";
import { Session } from "next-auth";

const authRoutes = ["/login", "/register"];
const notRequireEmailConfirmedRoutes = [
    "/success-register",
    "/confirm-email",
    "/choose-categories",
];
const checkProtectedRoute = (pathname: string) => {
    return (
        pathname.startsWith("/profile") ||
        pathname.startsWith("/my-courses") ||
        pathname === "/choose-categories"
    );
};

export const config = {
    matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};

export default auth(async function middleware(req) {
    const pathname = req.nextUrl.pathname;

    if (pathname === "/paypal-redirect") {
        return await handlePaypalRedirect(req);
    }

    const isLoggedIn = !!req.auth;
    const isAuthRoute = authRoutes.includes(pathname);

    if (isAuthRoute) {
        if (isLoggedIn) {
            return Response.redirect(new URL("/", req.nextUrl));
        }

        return;
    }

    const isProtectedRoute = checkProtectedRoute(pathname);
    if (isProtectedRoute && !isLoggedIn) {
        return Response.redirect(
            new URL(`/login?callbackUrl=${pathname}`, req.nextUrl)
        );
    }

    if (!notRequireEmailConfirmedRoutes.includes(pathname)) {
        const user = req.auth
            ? await getAuthenticatedUser(req.auth.accessToken, "emailConfirmed")
            : null;

        if (user && !user.emailConfirmed) {
            return Response.redirect(new URL("/success-register", req.nextUrl));
        }
    }

    return;
});

async function handlePaypalRedirect(req: any) {
    const session = req.auth as Session | null;

    if (!session) {
        return Response.redirect(new URL(`/`, req.nextUrl));
    }

    const reqUrl = req.url as string;
    const { searchParams } = new URL(reqUrl);
    const code = searchParams.get("code");

    if (!code) {
        return Response.redirect(new URL(`/`, req.nextUrl));
    }

    try {
        await linkPaypalAccount(code, session.accessToken);

        return Response.redirect(new URL(`/profile/payouts`, req.nextUrl));
    } catch (error) {}

    return Response.redirect(new URL(`/`, req.nextUrl));
}
