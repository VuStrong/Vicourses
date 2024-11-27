import { Column, Entity, ManyToOne, PrimaryColumn } from "typeorm";
import User from "./user.entity";

@Entity({ name: "refresh_tokens" })
export default class RefreshToken {
    @PrimaryColumn()
    userId: string

    @PrimaryColumn()
    token: string

    @ManyToOne(() => User, (user) => user.refreshTokens, { onDelete: "CASCADE" })
    user: User

    @Column()
    expiryTime: Date
}