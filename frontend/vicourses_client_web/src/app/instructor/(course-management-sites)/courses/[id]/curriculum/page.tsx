import SectionsContainer from "./SectionsContainer";

export default async function CourseCurriculumPage({
    params,
}: {
    params: { id: string };
}) {
    return (
        <div>
            <h1 className="text-gray-900 text-3xl mb-5">Curriculum</h1>
            <hr className="my-3 border-2" />

            <SectionsContainer courseId={params.id} />
        </div>
    );
}
