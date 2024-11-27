import { Router } from "express";
import validateRequest from "../middlewares/validate-request.middleware";
import authenticate from "../middlewares/authenticate.middleware";
import checkRole from "../middlewares/check-role.middleware";
import { Role } from "../entities/user.entity";
import * as usersController from "../controllers/users.controller";

const usersRoute = Router();

usersRoute.get(
    "/users",
    authenticate(),
    checkRole(Role.ADMIN),
    validateRequest("(get)/users", "query"),
    usersController.handleGetListOfUsers,
);

usersRoute.get(
    "/users/:id",
    authenticate(),
    checkRole(Role.ADMIN),
    usersController.handleGetUser,
);

usersRoute.get(
    "/users/:id/public-profile",
    authenticate(true),
    usersController.handleGetPublicProfile,
);

usersRoute.patch(
    "/users/:id/lock",
    authenticate(),
    checkRole(Role.ADMIN),
    validateRequest("(patch)/users/:id/lock"),
    usersController.handleLockUser,
);

export default usersRoute;