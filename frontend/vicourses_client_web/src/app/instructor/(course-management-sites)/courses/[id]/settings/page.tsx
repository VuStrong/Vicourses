import { notFound } from "next/navigation";
import { auth } from "@/libs/auth";
import { getCourseById } from "@/services/api/course";
import DeleteCourseButton from "./DeleteCourseButton";
import UnpublishCourseButton from "./UnpublishCourseButton";

export default async function CourseSettingsPage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    if (!course) notFound();

    return (
        <div>
            <h1 className="text-gray-900 text-3xl mb-5">Settings</h1>
            <hr className="my-3 border-2" />

            <div>
                <div className="text-black font-semibold">Course status</div>
                <div className="mb-5">
                    {course.status === "Unpublished" &&
                        "This course is not published on Vicourses."}
                    {course.status === "WaitingToVerify" &&
                        "This course is being verified by Vicourses."}
                    {course.status === "Published" &&
                        "This course is published on Vicourses."}
                </div>
                <div className="flex flex-col md:flex-row gap-5 items-center mb-5">
                    <div className="w-full md:max-w-[16rem]">
                        <UnpublishCourseButton course={course} />
                    </div>
                    <div className="text-black">
                        New students can't find your course through search.
                        However, current students will still be able to access
                        the course content.
                    </div>
                </div>
                <div className="flex flex-col md:flex-row gap-5 items-center">
                    <div className="w-full md:max-w-[16rem]">
                        <DeleteCourseButton
                            courseId={course.id}
                            disabled={
                                course.status === "Published" ||
                                course.studentCount > 0 ||
                                course.metrics.sectionsCount > 0 ||
                                course.metrics.lessonsCount > 0
                            }
                        />
                    </div>
                    <div className="text-black">
                        We guarantee students lifetime access. Therefore, you
                        cannot delete a course after a student has enrolled.
                        Also if your course still contains resources (lessons,
                        quizzes, ...), you cannot delete it.
                    </div>
                </div>
            </div>
        </div>
    );
}
