import { auth } from "@/libs/auth";
import UpdateCourseOverviewForm from "./UpdateCourseOverviewForm";
import { getCourseById } from "@/services/api/course";
import { notFound } from "next/navigation";

export default async function CourseOverviewPage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    return (
        <div>
            <h1 className="text-gray-900 text-3xl mb-5">Overview</h1>
            <hr className="my-3 border-2" />

            <p>
                Once you're done with this section, think about creating a
                compelling Course Dashboard that shows why someone would want to
                enroll in your course.
            </p>

            <UpdateCourseOverviewForm course={course} />
        </div>
    );
}
