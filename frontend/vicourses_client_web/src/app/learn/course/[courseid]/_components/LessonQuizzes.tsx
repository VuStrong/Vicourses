"use client";

import { useState } from "react";
import { IoMdClose, IoMdCheckmark } from "react-icons/io";
import { Button, Checkbox } from "@material-tailwind/react";

import { Lesson, Quiz } from "@/libs/types/lesson";

type Answers = {
    [key: number]: number[] | undefined;
};

type Result = {
    corrects: Quiz[];
    incorrects: Quiz[];
    allCorrect: boolean;
};

export default function LessonQuizzes({ lesson }: { lesson: Lesson }) {
    const [currentQuiz, setCurrentQuiz] = useState<Quiz | undefined>(
        lesson.quizzes[0]
    );
    const [answers, setAnswers] = useState<Answers>({});
    const [result, setResult] = useState<Result>();

    const handleAnswerRadioChange = (number: number, checked: boolean) => {
        if (!currentQuiz) return;

        if (currentQuiz.isMultiChoice) {
            const currentQuizAnswers = answers[currentQuiz.number] || [];

            setAnswers({
                ...answers,
                [currentQuiz.number]: checked
                    ? [...currentQuizAnswers, number]
                    : currentQuizAnswers.filter((n) => n !== number),
            });
        } else {
            setAnswers({
                ...answers,
                [currentQuiz.number]: checked ? [number] : [],
            });
        }
    };

    const handleNextQuiz = () => {
        if (!currentQuiz) return;

        const nextQuiz = lesson.quizzes.find(
            (q) => q.number === currentQuiz.number + 1
        );

        if (nextQuiz) {
            setCurrentQuiz(nextQuiz);
        }
    };

    const handleCheckAnswers = () => {
        const result: Result = {
            corrects: [],
            incorrects: [],
            allCorrect: true,
        };

        Object.entries(answers).forEach(([key, value]) => {
            const quiz = lesson.quizzes.find((q) => q.number === +key);

            if (!quiz || !value) return;

            if (quiz.isMultiChoice) {
                const correctAnswers = quiz.answers.filter((a) => a.isCorrect);

                if (value.length !== correctAnswers.length) {
                    result.allCorrect = false;
                    result.incorrects.push(quiz);
                    return;
                }

                const allCorrect = correctAnswers.every((answer) =>
                    value.includes(answer.number)
                );

                if (allCorrect) {
                    result.corrects.push(quiz);
                } else {
                    result.allCorrect = false;
                    result.incorrects.push(quiz);
                }
            } else {
                const correctAnswer = quiz.answers.find((a) => a.isCorrect);

                if (value[0] === correctAnswer?.number) {
                    result.corrects.push(quiz);
                } else {
                    result.allCorrect = false;
                    result.incorrects.push(quiz);
                }
            }
        });

        setResult(result);
    };

    const handleTryAgain = () => {
        setResult(undefined);
        setAnswers({});
        setCurrentQuiz(lesson.quizzes[0]);
    };

    return (
        <>
            <div className="w-full h-96 overflow-y-scroll">
                {result && (
                    <div className="w-full lg:max-w-[70%] p-5 lg:px-0 mx-auto">
                        {!result.allCorrect ? (
                            <div className="text-2xl font-semibold text-black">
                                You answered {result.corrects.length}/
                                {lesson.quizzesCount} questions correctly.
                            </div>
                        ) : (
                            <div className="text-2xl font-semibold text-black">
                                Great!, you answered all questions correctly.
                            </div>
                        )}

                        {result.corrects.length > 0 && (
                            <>
                                <div className="flex gap-2 items-center mt-5 font-semibold">
                                    <div className="text-green-600">
                                        <IoMdCheckmark />
                                    </div>{" "}
                                    Correct quizzes
                                </div>
                                {result.corrects.map((correctQuiz) => (
                                    <div
                                        key={correctQuiz.number}
                                        className="pl-6"
                                    >
                                        Quiz {correctQuiz.number}.
                                    </div>
                                ))}
                            </>
                        )}

                        {result.incorrects.length > 0 && (
                            <>
                                <div className="flex gap-2 items-center mt-5 font-semibold">
                                    <div className="text-red-600">
                                        <IoMdClose />
                                    </div>{" "}
                                    Incorrect quizzes
                                </div>
                                {result.incorrects.map((incorrectQuiz) => (
                                    <div
                                        key={incorrectQuiz.number}
                                        className="pl-6"
                                    >
                                        Quiz {incorrectQuiz.number}.
                                    </div>
                                ))}
                            </>
                        )}
                    </div>
                )}

                {!result && currentQuiz && (
                    <div className="w-full lg:max-w-[70%] p-5 lg:px-0 mx-auto">
                        <div className="font-semibold mb-3">
                            Quiz number {currentQuiz.number}/
                            {lesson.quizzesCount}
                        </div>
                        <div
                            className="mb-3"
                            dangerouslySetInnerHTML={{
                                __html: currentQuiz.title,
                            }}
                        />
                        <div className="flex flex-col gap-3">
                            {currentQuiz.answers.map((answer) => (
                                <label
                                    key={answer.number}
                                    htmlFor={`answer-${answer.number}`}
                                    className="border border-gray-700 px-3 flex gap-3 items-center cursor-pointer"
                                >
                                    <Checkbox
                                        crossOrigin={undefined}
                                        id={`answer-${answer.number}`}
                                        checked={
                                            answers[currentQuiz.number]
                                                ? answers[
                                                      currentQuiz.number
                                                  ]!.includes(answer.number)
                                                : false
                                        }
                                        onChange={(e) =>
                                            handleAnswerRadioChange(
                                                answer.number,
                                                e.target.checked
                                            )
                                        }
                                    />
                                    {answer.title}
                                </label>
                            ))}
                        </div>
                    </div>
                )}
            </div>
            {currentQuiz && (
                <div className="flex items-center justify-between p-3 border-y border-gray-700">
                    <div className="font-semibold">
                        Quiz number {currentQuiz.number}/{lesson.quizzesCount}
                    </div>
                    <div>
                        {result && (
                            <Button
                                className="rounded-none"
                                size="sm"
                                onClick={handleTryAgain}
                            >
                                Try again
                            </Button>
                        )}

                        {!result &&
                            currentQuiz.number === lesson.quizzesCount && (
                                <Button
                                    className="rounded-none"
                                    size="sm"
                                    disabled={
                                        !answers[currentQuiz.number] ||
                                        answers[currentQuiz.number]!.length ===
                                            0
                                    }
                                    onClick={handleCheckAnswers}
                                >
                                    Check answers
                                </Button>
                            )}

                        {currentQuiz.number !== lesson.quizzesCount && (
                            <Button
                                className="rounded-none"
                                size="sm"
                                disabled={
                                    !answers[currentQuiz.number] ||
                                    answers[currentQuiz.number]!.length === 0
                                }
                                onClick={handleNextQuiz}
                            >
                                Next quiz
                            </Button>
                        )}
                    </div>
                </div>
            )}
        </>
    );
}
