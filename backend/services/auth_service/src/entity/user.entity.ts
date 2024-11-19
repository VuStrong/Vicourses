import { Entity, Column, PrimaryGeneratedColumn, CreateDateColumn } from "typeorm"

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

    @Column()
    passwordHash: string

    @Column({ default: false })
    emailConfirmed: boolean

    @Column({ type: "datetime", default: null })
    lockoutEnd: Date | null

    @Column({
        type: "enum",
        enum: Role,
        default: Role.STUDENT,
    })
    role: Role

    toJSON() {
        return {
            id: this.id,
            name: this.name,
            email: this.email,
            createdAt: this.createdAt,
            role: this.role,
            emailConfirmed: this.emailConfirmed,
            lockoutEnd: this.lockoutEnd,
            locked: this.isLocked(),
        }
    }

    public isLocked(): boolean {
        return this.lockoutEnd !== null && this.lockoutEnd > new Date()
    }
}
