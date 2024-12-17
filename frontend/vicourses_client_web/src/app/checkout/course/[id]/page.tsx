import { Metadata } from "next";
import { notFound, redirect } from "next/navigation";
import { auth } from "@/libs/auth";
import { checkEnroll, getCourseById } from "@/services/api/course";
import CheckoutOverview from "./CheckoutOverview";
import CheckoutSummary from "./CheckoutSummary";

export const metadata: Metadata = {
    title: "Checkout | Vicourses",
};

export default async function CheckoutPage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    if (!session) redirect("/login");

    const enrolled = await checkEnroll(params.id, session.accessToken);

    if (enrolled) redirect(`/learn/course/${params.id}`);

    const course = await getCourseById(params.id, session.accessToken);

    if (!course) notFound();

    return (
        <div className="lg:flex justify-center gap-10 mb-20">
            <div className="lg:min-w-[33rem]">
                <CheckoutOverview course={course} />
            </div>
            <div className="bg-[#f7f9fa] lg:w-[20rem] px-0 lg:px-10 mt-20 lg:mt-0">
                <CheckoutSummary course={course} />
            </div>
        </div>
    );
}
