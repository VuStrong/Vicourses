import { Request, Response, Router } from "express";
import swaggerUi, { SwaggerUiOptions } from "swagger-ui-express";
import authRoute from "./auth.route";
import swaggerDocV1 from "../../swagger/swagger-v1.json";

var options: SwaggerUiOptions = {
    explorer: true,
    swaggerOptions: {
        urls: [
            {
                url: "/swagger/v1/swagger.json",
                name: "Auth API - v1",
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

router.get('/hc', async (_, res, __) => {
    const healthcheck = {
        uptime: process.uptime(),
        status: 'Healthy',
        timestamp: Date.now()
    };
    try {
        res.send(healthcheck);
    } catch (error) {
        res.status(503).send();
    }
});

router.use("/api/as/v1/auth", authRoute);

export default router;
