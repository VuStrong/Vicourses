import { Request, Response, Router } from "express";
import swaggerUi, { SwaggerUiOptions } from "swagger-ui-express";
import swaggerDocV1 from "../../swagger/swagger-v1.json";
import { handleHealthCheck } from "../controllers/healthchecks.controller";
import authRoute from "./auth.route";
import meRoute from "./me.route";
import usersRoute from "./users.route";

var options: SwaggerUiOptions = {
    explorer: true,
    swaggerOptions: {
        urls: [
            {
                url: "/swagger/v1/swagger.json",
                name: "User API - v1",
            },
        ],
    },
};

const router = Router();

router.get("/swagger/v1/swagger.json", (req: Request, res: Response) => {
    res.setHeader("Content-Type", "application/json");
    res.send(swaggerDocV1);
});
router.use(
    "/swagger",
    swaggerUi.serveFiles(undefined, options),
    swaggerUi.setup(undefined, options)
);

router.get('/hc', handleHealthCheck);

const apiRouter = Router()
    .use(authRoute)
    .use(meRoute)
    .use(usersRoute);

router.use("/api/us/v1", apiRouter);

export default router;