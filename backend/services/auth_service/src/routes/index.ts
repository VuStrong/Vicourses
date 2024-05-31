import { Request, Response, Router } from "express";
import swaggerUi from "swagger-ui-express";
import authRoute from "./auth.route";
import swaggerDoc from "../../swagger/swagger.json";

const router = Router();

router.use("/swagger", swaggerUi.serve);
router.get("/swagger", swaggerUi.setup(swaggerDoc));
router.get('/swagger.json', (req: Request, res: Response) => {
  res.setHeader('Content-Type', 'application/json');
  res.send(swaggerDoc);
});

router.use("/api/v1/auth", authRoute);

export default router;