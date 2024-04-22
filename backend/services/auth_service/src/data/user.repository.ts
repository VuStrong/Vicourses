import User from "../entity/user.entity"
import { dataSource } from "./data-source"

export const userRepository = dataSource.getRepository(User);