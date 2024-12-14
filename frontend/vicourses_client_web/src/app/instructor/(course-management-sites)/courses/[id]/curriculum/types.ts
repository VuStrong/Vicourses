export type CurriculumItem = {
    id: string;
    type: CurriculumItemType;
    sectionId?: string;
    title?: string;
}

export type CurriculumItemType = "Section" | "Lesson";