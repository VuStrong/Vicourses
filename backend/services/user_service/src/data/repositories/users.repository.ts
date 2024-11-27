import User from "../../entities/user.entity";
import { dataSource } from "../data-source";

export const usersRepository = dataSource.getRepository(User);
