import { create } from "zustand";

interface SidebarStore {
    isOpen: boolean;

    setIsOpen: (isOpen: boolean) => void;
    toggle: () => void;
}

const useSidebar = create<SidebarStore>((set) => ({
    isOpen: false,
    setIsOpen: (isOpen: boolean) => set({ isOpen }),
    toggle: () => set((state) => ({ isOpen: !state.isOpen })),
}));

export default useSidebar;
