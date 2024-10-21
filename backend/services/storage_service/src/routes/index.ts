import { Request, Response, Router } from "express";
import multer from "multer";
import { body } from "express-validator";
import swaggerUi, { SwaggerUiOptions } from "swagger-ui-express";
import swaggerDocV1 from "../../swagger/swagger-v1.json";
import * as uploadController from "../controllers/upload.controller";
import { AppError } from "../utils/app-error";
import authenticate from "../middlewares/authentice.middleware";
import { checkValidationResult } from "../middlewares/validators.middleware";

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
    },
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
    [
        body("fileId", "fileId is invalid")
            .optional()
            .matches(/\.(jpg|jpeg|png)$/i),
    ],
    checkValidationResult,
    uploadController.handleUploadImage
);

router.post(
    "/api/sts/v1/initialize-multipart-upload",
    authenticate,
    [
        body("fileId", "fileId is invalid").optional().isString(),
        body("fileName", "fileName is invalid").optional().isString(),
        body(
            "partCount",
            "partCount must be an integer, between 1 and 999"
        ).isInt({ min: 1, max: 999 }),
    ],
    checkValidationResult,
    uploadController.handleInitializeS3MultipartUpload
);

router.post(
    "/api/sts/v1/complete-multipart-upload",
    authenticate,
    [
        body("fileId", "fileId is missing").not().isEmpty(),
        body("fileId", "fileId is invalid").isString(),
        body("uploadId", "uploadId is missing").not().isEmpty(),
        body("uploadId", "uploadId is invalid").isString(),
        body("parts", "parts must be an array and not empty").isArray({
            min: 1,
        }),
        body("parts.*.PartNumber", "parts.PartNumber is invalid").isInt(),
        body("parts.*.ETag", "parts.ETag is invalid").isString(),
    ],
    checkValidationResult,
    uploadController.handleCompleteS3MultipartUpload
);

router.post(
    "/api/sts/v1/abort-multipart-upload",
    authenticate,
    [
        body("fileId", "fileId is missing").not().isEmpty(),
        body("fileId", "fileId is invalid").isString(),
        body("uploadId", "uploadId is missing").not().isEmpty(),
        body("uploadId", "uploadId is invalid").isString(),
    ],
    checkValidationResult,
    uploadController.handleAbortS3MultipartUpload
);

export default router;
