import { create } from "zustand";
import { CurriculumItem, CurriculumItemType } from "./types";

interface DeleteCurriculumItemModalStore {
    isOpen: boolean;
    item?: CurriculumItem;

    open: (item: CurriculumItem) => void;
    close: () => void;
}

const useDeleteCurriculumItemModal = create<DeleteCurriculumItemModalStore>(
    (set) => ({
        isOpen: false,
        itemId: undefined,
        itemType: undefined,

        open(item: CurriculumItem) {
            set({
                isOpen: true,
                item,
            });
        },
        close() {
            set({
                isOpen: false,
                item: undefined,
            });
        },
    })
);

export default useDeleteCurriculumItemModal;
