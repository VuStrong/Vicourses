import path from "path"
import { DataSource } from "typeorm"
import Config from "../config"

export const dataSource = new DataSource({
    type: "mysql",
    host: Config.DB_HOST,
    port: Config.DB_PORT,
    username: Config.DB_USER,
    password: Config.DB_PASS,
    database: Config.DB_DATABASE,
    entities: [path.join(__dirname, "..", "entity/*.{ts,js}")],
    logging: Config.NODE_ENV === "development",
    synchronize: false,
    migrations: [path.join(__dirname, "..", "migrations/*.{ts,js}")],
})