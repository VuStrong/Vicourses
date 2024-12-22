import { FaCheck } from "react-icons/fa";
import { MdOutlineClose } from "react-icons/md";
import { Lesson } from "../../types/course";

export default function LessonQuizzes({ lesson }: { lesson: Lesson }) {
    if (lesson.quizzesCount === 0) {
        return <div className="text-center">This lesson have no quizzes</div>;
    }

    return (
        <div className="flex flex-col gap-5">
            {lesson.quizzes.map((quiz) => (
                <div key={quiz.number} className="border-b pb-5">
                    <div className="font-semibold">
                        Quiz number: {quiz.number}
                    </div>
                    <div
                        className="mb-3"
                        dangerouslySetInnerHTML={{
                            __html: quiz.title,
                        }}
                    />
                    {quiz.answers.map((answer) => (
                        <div key={answer.number} className="flex gap-2">
                            {answer.isCorrect ? (
                                <div className="text-green-500">
                                    <FaCheck />
                                </div>
                            ) : (
                                <div className="text-red-500">
                                    <MdOutlineClose />
                                </div>
                            )}
                            <div className="font-semibold">
                                {answer.number}:{" "}
                            </div>
                            <div className="flex-grow">{answer.title}</div>
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
}
