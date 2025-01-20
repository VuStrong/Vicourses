"use client";

import { createContext, useContext, useRef } from "react";
import { createStore, StoreApi, useStore } from "zustand";
import { PublicCurriculum } from "@/libs/types/course";
import { getPublicCurriculum } from "@/services/api/course";

interface LessonInPublicCurriculum {
    id: string;
    title: string;
}

interface CurriculumState {
    isLoadingCurriculum: boolean;
    curriculum: PublicCurriculum | null;
    lessons: LessonInPublicCurriculum[];
    prevLesson: LessonInPublicCurriculum | null;
    currentLesson: LessonInPublicCurriculum | null;
    nextLesson: LessonInPublicCurriculum | null;

    fetchCurriculum: (
        courseId: string,
        accessToken: string,
        initialLessonId?: string
    ) => void;
    setCurrentLesson: (lessonId: string) => void;
}

const getLessonsFromCurriculum = (curriculum: PublicCurriculum | null) => {
    const lessons: LessonInPublicCurriculum[] = [];
    if (!curriculum) return lessons;

    curriculum.sections.forEach((section) => {
        section.lessons.forEach((lesson) => {
            lessons.push({
                id: lesson.id,
                title: lesson.title,
            });
        });
    });

    return lessons;
};

const CurriculumStoreContext = createContext<StoreApi<CurriculumState> | null>(
    null
);

const createCurriculumStore = () => createStore<CurriculumState>((set) => ({
    isLoadingCurriculum: false,
    curriculum: null,
    lessons: [],
    prevLesson: null,
    currentLesson: null,
    nextLesson: null,

    fetchCurriculum: (
        courseId: string,
        accessToken: string,
        initialLessonId?: string
    ) => {
        (async () => {
            set({ isLoadingCurriculum: true });

            const curriculum = await getPublicCurriculum(courseId, accessToken);
            const lessons = getLessonsFromCurriculum(curriculum);
            let prevLesson = null;
            let currentLesson = null;
            let nextLesson = null;

            if (initialLessonId) {
                const currentIndex = lessons.findIndex(
                    (l) => l.id === initialLessonId
                );

                if (currentIndex >= 0) {
                    currentLesson = lessons[currentIndex];

                    if (currentIndex > 0) {
                        prevLesson = lessons[currentIndex - 1];
                    }
                    if (currentIndex < lessons.length - 1) {
                        nextLesson = lessons[currentIndex + 1];
                    }
                }
            } else if (lessons.length > 0) {
                currentLesson = lessons[0];
                nextLesson = lessons[1] || null;
            }

            if (currentLesson) {
                const url = new URL(window.location.href);
                url.searchParams.set("lesson", currentLesson.id);
                window.history.replaceState(null, "", url.toString());
            }

            set({
                isLoadingCurriculum: false,
                curriculum,
                lessons,
                prevLesson,
                currentLesson,
                nextLesson,
            });
        })();
    },

    setCurrentLesson: (lessonId: string) =>
        set((state) => {
            if (lessonId === state.currentLesson?.id) return {};

            const lessons = state.lessons;
            const index = lessons.findIndex((l) => l.id === lessonId);

            if (index < 0) return {};

            let currentLesson = lessons[index];
            let prevLesson = null;
            let nextLesson = null;

            if (index > 0) {
                prevLesson = lessons[index - 1];
            }
            if (index < lessons.length - 1) {
                nextLesson = lessons[index + 1];
            }

            if (currentLesson) {
                const url = new URL(window.location.href);
                url.searchParams.set("lesson", currentLesson.id);
                window.history.replaceState(null, "", url.toString());
            }

            return {
                prevLesson,
                currentLesson,
                nextLesson,
            };
        }),
}));

export const CurriculumStoreProvider = ({
    children,
}: {
    children: React.ReactNode;
}) => {
    const storeRef = useRef<StoreApi<CurriculumState>>();
    if (!storeRef.current) {
        storeRef.current = createCurriculumStore();
    }

    return (
        <CurriculumStoreContext.Provider value={storeRef.current}>
            {children}
        </CurriculumStoreContext.Provider>
    );
};

export default function useCurriculum<T>(
    selector: (state: CurriculumState) => T
) {
    const store = useContext(CurriculumStoreContext);
    if (!store) {
        throw new Error("Missing StoreProvider");
    }
    return useStore(store, selector);
}
