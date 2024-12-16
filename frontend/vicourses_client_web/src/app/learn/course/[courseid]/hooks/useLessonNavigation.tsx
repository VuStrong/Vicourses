import { create } from "zustand";
import { PublicCurriculum } from "@/libs/types/course";

interface LessonNavigationStore {
    lessonIds: string[];
    prevId?: string;
    nextId?: string;

    setIdsFromPublicCurriculum: (
        curriculum: PublicCurriculum,
        lessonId?: string
    ) => void;

    setCurrentId: (lessonId: string) => void;
}

const useLessonNavigation = create<LessonNavigationStore>((set) => ({
    lessonIds: [],
    prevId: undefined,
    nextId: undefined,

    setIdsFromPublicCurriculum: (
        curriculum: PublicCurriculum,
        lessonId?: string
    ) => {
        const ids: string[] = [];

        curriculum.sections.forEach((section) => {
            section.lessons.forEach((lesson) => {
                ids.push(lesson.id);
            });
        });

        let prevId, nextId;
        if (lessonId) {
            const index = ids.findIndex((id) => id === lessonId);
            if (index > 0) {
                prevId = ids[index - 1];
            }
            if (index < ids.length - 1) {
                nextId = ids[index + 1];
            }
        }

        set({ lessonIds: ids, prevId, nextId });
    },

    setCurrentId: (lessonId: string) =>
        set((state) => {
            const lessonIds = state.lessonIds;
            const index = lessonIds.findIndex((id) => id === lessonId);

            if (index < 0) {
                return {};
            }

            return {
                prevId: index > 0 ? lessonIds[index - 1] : undefined,
                nextId:
                    index < lessonIds.length - 1
                        ? lessonIds[index + 1]
                        : undefined,
            };
        }),
}));

export default useLessonNavigation;
