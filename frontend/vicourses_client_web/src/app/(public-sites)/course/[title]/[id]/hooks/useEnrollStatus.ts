import { create } from "zustand";
import { checkEnroll } from "@/services/api/course";

interface EnrollStatusStore {
    isLoading: boolean;
    enrolled: boolean;

    check: (courseId: string, accessToken?: string) => Promise<void>;
}

const useEnrollStatus = create<EnrollStatusStore>((set) => ({
    isLoading: true,
    enrolled: false,

    check: async (courseId: string, accessToken?: string) => {
        if (!accessToken) {
            set({ isLoading: false, enrolled: false });
            return;
        }

        set({ isLoading: true, enrolled: false });

        const enrolled = await checkEnroll(courseId, accessToken);

        set({ isLoading: false, enrolled });
    },
}));

export default useEnrollStatus;
