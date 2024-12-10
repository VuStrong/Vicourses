import { auth } from "@/libs/auth";
import { getCourseById } from "@/services/api/course";
import { notFound } from "next/navigation";
import GoalsForm from "./GoalsForm";

export default async function CourseGoalsPage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    return (
        <div>
            <h1 className="text-gray-900 text-3xl mb-5">Goals</h1>
            <hr className="my-3 border-2" />

            <p>
                The following descriptions will be publicly visible on your
                course overview page and will directly impact course performance and
                help students decide if the course is right for them.
            </p>

            <GoalsForm course={course} />
        </div>
    );
}
