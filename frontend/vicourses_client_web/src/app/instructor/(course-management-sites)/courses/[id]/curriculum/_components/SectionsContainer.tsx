"use client";

import { Session } from "next-auth";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import {
    DndContext,
    closestCenter,
    KeyboardSensor,
    PointerSensor,
    useSensor,
    useSensors,
    DragEndEvent,
    DragOverlay,
    DragStartEvent,
} from "@dnd-kit/core";
import {
    arrayMove,
    SortableContext,
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from "@dnd-kit/sortable";

import {
    getInstructorCurriculum,
    updateCurriculumOrder,
} from "@/services/api/course";
import { UpdateCurriculumRequest } from "@/libs/types/course";
import { Section, SectionInInstructorCurriculum } from "@/libs/types/section";
import { Lesson } from "@/libs/types/lesson";
import { Loader } from "@/components/common";
import SectionItem from "./section/SectionItem";
import AddSectionButton from "./section/AddSectionButton";
import DeleteCurriculumItemModal from "./DeleteCurriculumItemModal";
import { CurriculumItem } from "../_lib/types";
import { SectionItemOverlay, LessonItemOverlay } from "./Overlays";

export default function SectionsContainer({ courseId }: { courseId: string }) {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [sections, setSections] = useState<SectionInInstructorCurriculum[]>(
        []
    );
    const { data: session, status } = useSession();
    const [activeItem, setActiveItem] = useState<CurriculumItem>();

    const sensors = useSensors(
        useSensor(PointerSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setIsLoading(true);

                const result = await getInstructorCurriculum(
                    courseId,
                    session.accessToken
                );

                setSections(result?.sections || []);
                setIsLoading(false);
            })();
        }
    }, [status]);

    if (isLoading) {
        return (
            <div className="flex justify-center">
                <Loader />
            </div>
        );
    }

    return (
        <div className="flex flex-col gap-10">
            <DeleteCurriculumItemModal
                session={session}
                onItemDeleted={handleItemDeleted}
            />

            {sections.length > 0 ? (
                <DndContext
                    sensors={sensors}
                    collisionDetection={closestCenter}
                    onDragStart={handleDragStart}
                    onDragEnd={handleDragEnd}
                >
                    <SortableContext
                        items={sections}
                        strategy={verticalListSortingStrategy}
                    >
                        {sections.map((section, index) => (
                            <SectionItem
                                key={section.id}
                                section={section}
                                index={index + 1}
                                session={session}
                                onLessonAdded={handleLessonAdded}
                            />
                        ))}
                    </SortableContext>
                    <DragOverlay>{getDragOverlay()}</DragOverlay>
                </DndContext>
            ) : (
                <div className="flex justify-center">No curriculum</div>
            )}

            <div>
                <AddSectionButton
                    session={session}
                    courseId={courseId}
                    onSectionAdded={handleSectionAdded}
                />
            </div>
        </div>
    );

    function handleDragStart(event: DragStartEvent) {
        const { active } = event;
        const data = active.data.current;
        setActiveItem({
            id: active.id as string,
            type: data!.type,
            sectionId: data!.sectionId,
            title: data!.title,
        });
    }

    function handleDragEnd(event: DragEndEvent) {
        setActiveItem(undefined);

        const { active, over } = event;

        if (!over) return;

        if (active.id === over.id) return;

        const activeItem: CurriculumItem = {
            id: active.id as string,
            type: active.data.current!.type,
            sectionId: active.data.current!.sectionId,
        };
        const overItem: CurriculumItem = {
            id: over.id as string,
            type: over.data.current!.type,
            sectionId: over.data.current!.sectionId,
        };

        let newSections: SectionInInstructorCurriculum[];

        // if activeItem is Section, just move 2 sections
        // if activeItem is Lesson, remove it from its Section and add to overItem's Section
        if (activeItem.type === "Section") {
            let overId: string;
            if (overItem.type === "Lesson") {
                overId = overItem.sectionId || "";
            } else {
                overId = overItem.id;
            }

            if (activeItem.id === overId) return;

            const oldIndex = sections.findIndex((s) => s.id === activeItem.id);
            const newIndex = sections.findIndex((s) => s.id === overId);

            newSections = arrayMove(sections, oldIndex, newIndex);
        } else {
            const activeItemSection = sections.find(
                (s) => s.id === activeItem.sectionId
            );
            const overId =
                overItem.type === "Section" ? overItem.id : overItem.sectionId;
            const overItemSection = sections.find((s) => s.id === overId);

            if (!activeItemSection || !overItemSection) return;

            const activeItemIndex = activeItemSection.lessons.findIndex(
                (l) => l.id === activeItem.id
            );
            const overItemIndex =
                overItem.type === "Section"
                    ? 0
                    : overItemSection.lessons.findIndex(
                          (l) => l.id === overItem.id
                      );
            if (activeItemIndex < 0 || overItemIndex < 0) return;

            const [removedLesson] = activeItemSection.lessons.splice(
                activeItemIndex,
                1
            );
            removedLesson.sectionId = overItemSection.id;
            overItemSection.lessons.splice(overItemIndex, 0, removedLesson);

            newSections = [...sections];
        }

        setSections(newSections);
        reorderCurriculum(newSections, courseId, session);
    }

    function getDragOverlay() {
        if (!activeItem) return;

        if (activeItem.type === "Section") {
            return <SectionItemOverlay title={activeItem.title || ""} />;
        }

        return <LessonItemOverlay title={activeItem.title || ""} />;
    }

    function handleSectionAdded(section: Section) {
        var newSections = [
            ...sections,
            {
                ...section,
                duration: 0,
                lessonCount: 0,
                lessons: [],
            },
        ];

        setSections(newSections);
        reorderCurriculum(newSections, courseId, session);
    }

    function handleLessonAdded(lesson: Lesson) {
        const newSections = sections.map((section) => {
            if (section.id === lesson.sectionId) {
                section.lessons.push(lesson);
            }

            return section;
        });

        setSections(newSections);
        reorderCurriculum(newSections, courseId, session);
    }

    function handleItemDeleted(item: CurriculumItem) {
        let newSections: SectionInInstructorCurriculum[];

        if (item.type === "Section") {
            newSections = sections.filter((s) => s.id !== item.id);
        } else {
            newSections = sections.map((s) => {
                if (s.id !== item.sectionId) return s;

                return {
                    ...s,
                    lessons: s.lessons.filter((l) => l.id !== item.id),
                };
            });
        }

        setSections(newSections);
        reorderCurriculum(newSections, courseId, session);
    }
}

async function reorderCurriculum(
    sections: SectionInInstructorCurriculum[],
    courseId: string,
    session: Session | null
) {
    const request: UpdateCurriculumRequest = {
        items: [],
    };

    sections.forEach((section) => {
        request.items.push({
            id: section.id,
            type: "Section",
        });

        section.lessons.forEach((lesson) => {
            request.items.push({
                id: lesson.id,
                type: "Lesson",
            });
        });
    });

    await updateCurriculumOrder(courseId, request, session?.accessToken || "");
}
