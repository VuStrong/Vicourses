import { Column, Entity, Index, PrimaryColumn } from "typeorm";

export enum TokenType {
    REFRESH_TOKEN = "refresh_token",
    CONFIRM_EMAIL = "comfirm_email",
    RESET_PASSWORD = "reset_password",
}

@Entity({ name: "tokens" })
export default class Token {
    @PrimaryColumn()
    token: string

    @PrimaryColumn()
    @Index()
    userId: string

    @Column({
        type: "enum",
        enum: TokenType,
    })
    type: TokenType

    @Column()
    expiryTime: Date
}