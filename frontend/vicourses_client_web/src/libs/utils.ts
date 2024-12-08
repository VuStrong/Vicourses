export function getFileExtension(file: File): string {
    return file.name.split(".").pop() || "";
}

export function formatLength(length: number) {
    const second = length % 60;
    const minute = Math.floor(length / 60) % 60;
    const hour = Math.floor(length / 60 / 60);

    const hourStr = hour < 10 ? `0${hour}` : `${hour}`;
    const minuteStr = minute < 10 ? `0${minute}` : `${minute}`;
    const secondStr = second < 10 ? `0${second}` : `${second}`;

    return hour === 0 ?
         `${minuteStr}:${secondStr}` :
         `${hourStr}:${minuteStr}:${secondStr}`;
}