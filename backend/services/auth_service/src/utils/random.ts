export function generateRandomString(length: number) {
    const chars =
        "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
    const charsLength = chars.length;

    const randomArray = Array.from(
        { length },
        () => chars[Math.floor(Math.random() * charsLength)]
    );

    const randomString = randomArray.join("");
    return randomString;
}
