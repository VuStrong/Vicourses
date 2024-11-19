import { Router } from "express";
import { body } from "express-validator";
import authenticate from "../middlewares/authenticate.middleware";
import * as usersController from "../controllers/users.controller";
import { checkValidationResult } from "../middlewares/validators.middleware";
import checkRole from "../middlewares/check-role.middleware";
import { Role } from "../entity/user.entity";

const usersRoute = Router();

usersRoute.get(
    "/:id", 
    authenticate,
    checkRole(Role.ADMIN),
    usersController.handleGetUser,
);

usersRoute.put(
    "/:id/lock", 
    authenticate,
    checkRole(Role.ADMIN),
    [
        body("days").isInt().withMessage("days must be an integer"),
    ],
    checkValidationResult,
    usersController.handleLockUser,
);

usersRoute.put(
    "/:id/role",
    authenticate,
    checkRole(Role.ADMIN),
    [
        body("role").isString().isIn([Role.ADMIN, Role.INSTRUCTOR, Role.STUDENT]).withMessage("role is invalid")
    ],
    checkValidationResult,
    usersController.handleSetUserRole,
);

export default usersRoute;