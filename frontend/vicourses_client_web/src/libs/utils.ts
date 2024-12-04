export function getFileExtension(file: File): string {
    return file.name.split(".").pop() || "";
}