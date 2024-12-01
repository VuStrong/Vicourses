import { 
    Entity, 
    Column, 
    PrimaryGeneratedColumn, 
    CreateDateColumn, 
    OneToMany, 
    Index, 
    OneToOne, 
    JoinColumn
} from "typeorm"
import RefreshToken from "./refresh-token.entity"
import PaypalAccount from "./paypal-account.entity"

export enum Role {
    ADMIN = "admin",
    INSTRUCTOR = "instructor",
    STUDENT = "student",
}

@Entity({ name: "users" })
export default class User {
    @PrimaryGeneratedColumn("uuid")
    id: string

    @CreateDateColumn()
    createdAt: Date

    @Column()
    name: string

    @Column({ unique: true })
    email: string

    @Column({ type: "varchar", default: null })
    hashedPassword: string | null

    @Column({ default: false })
    emailConfirmed: boolean

    @Column({ type: "varchar", default: null })
    emailConfirmationToken: string | null

    @Column({ type: "datetime", default: null })
    emailConfirmationTokenExpiryTime: Date | null

    @Column({ type: "varchar", default: null })
    passwordResetToken: string | null

    @Column({ type: "datetime", default: null })
    passwordResetTokenExpiryTime: Date | null

    @Column({ type: "datetime", default: null })
    lockoutEnd: Date | null

    @Column({
        type: "enum",
        enum: Role,
        default: Role.STUDENT,
    })
    @Index()
    role: Role

    @Column({ type: "varchar", default: null })
    thumbnailId: string | null

    @Column({ type: "varchar", default: null })
    thumbnailUrl: string | null

    @Column({ type: "varchar", default: null })
    headline: string | null

    @Column({ type: "text", default: null })
    description: string | null

    @Column({ type: "varchar", default: null })
    websiteUrl: string | null

    @Column({ type: "varchar", default: null })
    youtubeUrl: string | null

    @Column({ type: "varchar", default: null })
    facebookUrl: string | null

    @Column({ type: "varchar", default: null })
    linkedInUrl: string | null

    @Column({ default: true })
    enrolledCoursesVisible: boolean

    @Column({ default: true })
    isPublic: boolean

    @Column({ default: "" })
    courseTags: string

    @Column({ default: "" })
    categoryIds: string

    @Column({ default: 0 })
    totalEnrollmentCount: number;

    @OneToMany(() => RefreshToken, (token) => token.user)
    refreshTokens: RefreshToken[]

    @OneToOne(() => PaypalAccount, { nullable: true, onDelete: "SET NULL" })
    @JoinColumn()
    paypalAccount: PaypalAccount | null;

    public isLocked(): boolean {
        return this.lockoutEnd !== null && this.lockoutEnd > new Date()
    }
}