import { create } from "zustand";

interface WishlistStore {
    isLoading: boolean;

    courseIds: Set<string>;

    setIsLoading: (isLoading: boolean) => void;

    setCourseIds: (courseIds: string[]) => void;
    addCourseId: (courseId: string) => void;
    removeCourseId: (courseId: string) => void;
}

const useWishlist = create<WishlistStore>((set) => ({
    isLoading: false,
    setIsLoading: (isLoading: boolean) => set({ isLoading }),

    courseIds: new Set<string>(),

    setCourseIds: (courseIds: string[]) =>
        set({ courseIds: new Set(courseIds) }),

    addCourseId: (courseId: string) =>
        set((state) => ({
            courseIds: state.courseIds.add(courseId),
        })),

    removeCourseId: (courseId: string) =>
        set((state) => {
            state.courseIds.delete(courseId);

            return { courseIds: state.courseIds };
        }),
}));

export default useWishlist;