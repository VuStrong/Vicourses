import { create } from "zustand";

interface PreviewVideoModalStore {
    isOpen: boolean;

    open: () => Promise<void>;
    close: () => Promise<void>;
}

const usePreviewVideoModal = create<PreviewVideoModalStore>((set) => ({
    isOpen: false,

    open: async () => {
        set({ isOpen: true });
    },
    close: async () => {
        set({ isOpen: false });
    },
}));

export default usePreviewVideoModal;
