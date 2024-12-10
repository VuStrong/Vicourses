import { BACKEND_URL } from "@/libs/constants";
import { Locale } from "@/libs/types/common";

export async function getLocales(): Promise<Locale[]> {
    const res = await fetch(`${BACKEND_URL}/api/cs/v1/locales`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!res.ok) {
        return [];
    }

    const data = await res.json();
    return data;
}
