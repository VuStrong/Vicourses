import { auth } from "@/libs/auth";
import { getCourseById } from "@/services/api/course";
import { notFound } from "next/navigation";
import UpdatePriceForm from "./UpdatePriceForm";

export default async function CoursePricingPage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    return (
        <div>
            <h1 className="text-gray-900 text-3xl mb-5">Pricing</h1>
            <hr className="my-3 border-2" />

            <div className="text-black font-semibold">Set a price for your course</div>

            <UpdatePriceForm course={course} />
        </div>
    );
}
