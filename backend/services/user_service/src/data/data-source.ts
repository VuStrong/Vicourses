import path from "path"
import { DataSource } from "typeorm"
import Config from "../config"

export const dataSource = new DataSource({
    type: "mysql",
    host: Config.Database.Host,
    port: Config.Database.Port,
    username: Config.Database.User,
    password: Config.Database.Password,
    database: Config.Database.Name,
    entities: [path.join(__dirname, "..", "entities/*.{ts,js}")],
    logging: Config.Environment === "development",
    synchronize: false,
    migrations: [path.join(__dirname, "..", "migrations/*.{ts,js}")],
})