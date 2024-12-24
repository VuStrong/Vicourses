import { IoMdMenu } from "react-icons/io";

export function SectionItemOverlay({ title }: { title: string }) {
    return (
        <div className="bg-[#f7f9fa] border border-gray-700 py-5 px-2 opacity-80">
            <div className="flex justify-between">
                <div className="flex flex-col md:flex-row gap-2 items-center">
                    <span className="text-black font-semibold">Section: </span>
                    <span className="text-gray-800">{title}</span>
                </div>
                <div>
                    <IoMdMenu size={24} />
                </div>
            </div>
        </div>
    );
}

export function LessonItemOverlay({ title }: { title: string }) {
    return (
        <div className="bg-white border border-gray-700 py-5 px-2 opacity-80">
            <div className="flex justify-between">
                <div className="flex flex-col md:flex-row gap-2 items-center">
                    <span className="text-black font-semibold">Lesson: </span>
                    <span className="text-gray-800">{title}</span>
                </div>
                <div>
                    <IoMdMenu size={24} />
                </div>
            </div>
        </div>
    );
}
