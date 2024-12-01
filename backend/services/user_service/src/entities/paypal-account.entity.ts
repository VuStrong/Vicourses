import { Column, Entity, PrimaryGeneratedColumn } from "typeorm";

@Entity({ name: "paypal_accounts" })
export default class PaypalAccount {
    @PrimaryGeneratedColumn("uuid")
    id: string;

    @Column()
    payerId: string;

    @Column()
    email: string;

    @Column()
    refreshToken: string;

    @Column()
    emailVerified: boolean;

    @Column()
    verifiedAccount: boolean;
}