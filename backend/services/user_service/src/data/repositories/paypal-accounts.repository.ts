import PaypalAccount from "../../entities/paypal-account.entity";
import { dataSource } from "../data-source";

export const paypalAccountsRepository = dataSource.getRepository(PaypalAccount);