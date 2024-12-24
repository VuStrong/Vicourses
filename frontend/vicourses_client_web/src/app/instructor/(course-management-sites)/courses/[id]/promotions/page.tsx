import { notFound } from "next/navigation";
import { getCourseById } from "@/services/api/course";
import { auth } from "@/libs/auth";
import CreateCouponButton from "./_components/CreateCouponButton";
import ExpiredCouponsSection from "./_components/ExpiredCouponsSection";
import CouponActiveSection from "./_components/CouponActiveSection";

export default async function CoursePromotionsPage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    return (
        <div>
            <h1 className="text-gray-900 text-3xl mb-5">Promotions</h1>
            <hr className="my-3 border-2" />

            <CreateCouponButton course={course} />

            <CouponActiveSection course={course} />

            <ExpiredCouponsSection course={course} />
        </div>
    );
}
