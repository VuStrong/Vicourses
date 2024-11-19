import { Router } from "express";
import authenticate from "../middlewares/authenticate.middleware";
import * as meController from "../controllers/me.controller";
import { checkValidationResult, validateChangePasswordRequest } from "../middlewares/validators.middleware";

const meRoute = Router();

meRoute.get("/", authenticate, meController.handleGetUser);

meRoute.put(
    "/password", 
    authenticate,
    validateChangePasswordRequest(),
    checkValidationResult,
    meController.handleChangePassword,
);

export default meRoute;