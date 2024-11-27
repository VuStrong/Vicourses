import { FindOptionsOrder } from "typeorm"
import User, { Role } from "../entities/user.entity"

export type UpdateProfilePayload = {
    name?: string,
    thumbnailToken?: string,
    headline?: string,
    description?: string,
    websiteUrl?: string,
    youtubeUrl?: string,
    facebookUrl?: string,
    linkedInUrl?: string,
    enrolledCoursesVisible?: boolean,
    isPublic?: boolean,
    categoryIds?: string,
}

export type GetUsersPayload = {
    skip?: number,
    limit?: number,
    fields?: (keyof User)[],
    role?: Role,
    order?: FindOptionsOrder<User>
}