import { Router } from "express";
import validateRequest from "../middlewares/validate-request.middleware";
import authenticate from "../middlewares/authenticate.middleware";
import * as meController from "../controllers/me.controller";

const meRoute = Router();

meRoute.get(
    "/me",
    authenticate(),
    meController.handleGetAuthenticatedUser,
);

meRoute.patch(
    "/me",
    authenticate(),
    validateRequest("(patch)/me"),
    meController.handleUpdateUserProfile,
);

meRoute.patch(
    "/me/password",
    authenticate(),
    validateRequest("(patch)/me/password"),
    meController.handleChangePassword,
);

meRoute.post(
    "/me/paypal",
    authenticate(),
    validateRequest("(post)/me/paypal"),
    meController.handleLinkPaypalAccount,
);

meRoute.post(
    "/me/instructor",
    authenticate(),
    meController.handleUpdateRoleToInstructor,
);

export default meRoute;