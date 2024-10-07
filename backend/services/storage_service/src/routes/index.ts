import { Request, Response, Router } from "express";
import multer from "multer";
import swaggerUi, { SwaggerUiOptions } from "swagger-ui-express";
import swaggerDocV1 from "../../swagger/swagger-v1.json";
import * as uploadController from "../controllers/upload.controller";
import { AppError } from "../utils/app-error";
import authenticate from "../middlewares/authentice.middleware";

var swaggerUIOptions: SwaggerUiOptions = {
    explorer: true,
    swaggerOptions: {
        urls: [
            {
                url: "/swagger/v1/swagger.json",
                name: "Storage API - v1",
            },
        ],
    },
};

const storage = multer.memoryStorage();
const upload = multer({
    storage,
    limits: { fileSize: 10000000 },
    fileFilter: (_, file, cb) => {
        const imageMimeTypes = ["image/png", "image/jpg", "image/jpeg"];

        if (!imageMimeTypes.includes(file.mimetype)) {
            return cb(
                new AppError(
                    `Mimetype ${file.mimetype} is not allowed for image field`,
                    400
                )
            );
        }

        cb(null, true);
    }
}).single("image");

const router = Router();

router.get("/swagger/v1/swagger.json", (req: Request, res: Response) => {
    res.setHeader("Content-Type", "application/json");
    res.send(swaggerDocV1);
});
router.use(
    "/swagger",
    swaggerUi.serveFiles(undefined, swaggerUIOptions),
    swaggerUi.setup(undefined, swaggerUIOptions)
);

router.post(
    "/api/sts/v1/upload-image",
    authenticate,
    upload,
    uploadController.handleUploadImage,
);

router.post(
    "/api/sts/v1/initialize-multipart-upload",
    authenticate,
    uploadController.handleInitializeS3MultipartUpload,
);

router.post(
    "/api/sts/v1/complete-multipart-upload",
    authenticate,
    uploadController.handleCompleteS3MultipartUpload
);

router.post(
    "/api/sts/v1/abort-multipart-upload",
    authenticate,
    uploadController.handleAbortS3MultipartUpload
);

export default router;
