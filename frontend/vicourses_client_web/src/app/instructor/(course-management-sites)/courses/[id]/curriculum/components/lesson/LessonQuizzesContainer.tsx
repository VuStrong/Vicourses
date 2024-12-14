"use client";

import { useState } from "react";
import { Session } from "next-auth";
import { Button } from "@material-tailwind/react";
import toast from "react-hot-toast";

import { Loader } from "@/components/common";
import { AddOrUpdateQuizRequest, Lesson, Quiz } from "@/libs/types/lesson";
import {
    addQuizToLesson,
    removeQuizFromLesson,
    updateQuizInLesson,
} from "@/services/api/course-lesson";
import CreateUpdateQuizForm, { QuizFormValues } from "./CreateUpdateQuizForm";
import LessonQuizItem from "./LessonQuizItem";

export default function LessonQuizzesContainer({
    lessonId,
    quizzes,
    session,
    onQuizzesChanged,
}: {
    lessonId: string;
    quizzes: Quiz[];
    session: Session | null;
    onQuizzesChanged: (lesson: Lesson) => void;
}) {
    const [openQuizForm, setOpenQuizForm] = useState<boolean>(false);
    const [editingQuiz, setEditingQuiz] = useState<Quiz>();
    const [isDeleting, setIsDeleting] = useState<boolean>(false);

    const handleSubmitQuizForm = async (data: QuizFormValues) => {
        try {
            const request: AddOrUpdateQuizRequest = {
                title: data.title,
                answers: data.answers.map((a) => ({
                    ...a,
                    explanation: a.explanation ? a.explanation : undefined,
                })),
            };

            const updatedLesson = editingQuiz
                ? await updateQuizInLesson(
                      lessonId,
                      editingQuiz.number,
                      request,
                      session?.accessToken || ""
                  )
                : await addQuizToLesson(
                      lessonId,
                      request,
                      session?.accessToken || ""
                  );

            onQuizzesChanged(updatedLesson);

            return true;
        } catch (error: any) {
            toast.error(error.message);
            return false;
        }
    };

    const handleRemoveQuiz = async (quizNumber: number) => {
        setIsDeleting(true);

        try {
            const updatedLesson = await removeQuizFromLesson(
                lessonId,
                quizNumber,
                session?.accessToken || ""
            );

            onQuizzesChanged(updatedLesson);
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsDeleting(false);
    };

    const handleClickNewQuiz = () => {
        setEditingQuiz(undefined);
        setOpenQuizForm(true);
    };

    const handleClickEditQuiz = (quiz: Quiz) => {
        setEditingQuiz(quiz);
        setOpenQuizForm(true);
    };

    return (
        <>
            {openQuizForm && (
                <CreateUpdateQuizForm
                    submit={handleSubmitQuizForm}
                    cancel={() => setOpenQuizForm(false)}
                    initialQuizData={editingQuiz}
                />
            )}

            {!openQuizForm && (
                <>
                    <div className="flex gap-3 items-center justify-between mb-5 w-full">
                        <Button
                            variant="text"
                            size="sm"
                            className="border border-gray-700 rounded-none"
                            onClick={handleClickNewQuiz}
                        >
                            New quiz
                        </Button>
                        <a
                            href="#"
                            target="_blank"
                            className="bg-black py-2 px-3 text-white text-xs font-bold uppercase"
                        >
                            Preview
                        </a>
                    </div>
                    <div className="flex flex-col gap-3 relative">
                        {isDeleting && (
                            <div className="absolute w-full h-full bg-black bg-opacity-20 flex justify-center items-center">
                                <Loader color="#ffffff" />
                            </div>
                        )}

                        {quizzes.map((quiz) => (
                            <LessonQuizItem
                                key={quiz.number}
                                quiz={quiz}
                                deleteFn={async () =>
                                    await handleRemoveQuiz(quiz.number)
                                }
                                onClickEdit={() => handleClickEditQuiz(quiz)}
                            />
                        ))}
                    </div>
                </>
            )}
        </>
    );
}
