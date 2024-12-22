import { IoIosCheckmark } from "react-icons/io";

export default function CourseLearnedContents({
    contents,
}: {
    contents: string[];
}) {
    return (
        <div className="border border-gray-800 dark:border-gray-600 p-5">
            <h2 className="font-semibold text-2xl mb-5">Contents</h2>
            <div className="grid gap-3 md:grid-cols-2 grid-cols-1">
                {contents.map((content, index) => (
                    <div
                        key={index}
                        className="flex gap-2 items-center"
                    >
                        <IoIosCheckmark size={32} className="flex-shrink-0" />
                        <p className="flex-grow">{content}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}
