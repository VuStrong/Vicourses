export type Wishlist = {
    id: string;
    count: number;
    courses: CourseInWishlist[];
}

export type CourseInWishlist = {
    id: string;
    title: string;
    titleCleaned: string;
    isPaid: boolean;
    price: number;
    rating: number;
    thumbnailUrl: string | null;
    user: {
        id: string;
        name: string;
        thumbnailUrl: string | null;
    },
}