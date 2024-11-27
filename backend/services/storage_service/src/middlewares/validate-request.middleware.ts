import { Request, Response, NextFunction } from "express";
import Joi, { ObjectSchema } from "joi";

type RouteName =
    | "/api/sts/v1/upload-image"
    | "/api/sts/v1/initialize-multipart-upload"
    | "/api/sts/v1/complete-multipart-upload"
    | "/api/sts/v1/abort-multipart-upload";

export default function validateRequest(route: RouteName) {
    return function (req: Request, res: Response, next: NextFunction) {
        const schema = schemas[route];

        const validationResult = schema.validate(req.body, {
            allowUnknown: true,
        });

        if (validationResult.error) {
            const errors = validationResult.error.details.map((e) => e.message);

            return res.status(400).send({
                message: "Validation failed",
                code: 400,
                errors,
            });
        }

        next();
    };
}

const schemas: {
    [key in RouteName]: ObjectSchema;
} = {
    "/api/sts/v1/upload-image": Joi.object({
        fileId: Joi.string().max(100).regex(/\.(jpg|jpeg|png)$/i).optional(),
    }),
    
    "/api/sts/v1/initialize-multipart-upload": Joi.object({
        fileId: Joi.string().max(100).optional(),
        fileName: Joi.string().max(200).optional(),
        partCount: Joi.number().integer().min(1).max(999).required(),
    }),

    "/api/sts/v1/complete-multipart-upload": Joi.object({
        fileId: Joi.string().required(),
        uploadId: Joi.string().required(),
        parts: Joi
            .array()
            .min(1)
            .items(Joi.object({
                PartNumber: Joi.number().integer().min(0).required(),
                ETag: Joi.string().required(),
            }))
            .required(),
    }),

    "/api/sts/v1/abort-multipart-upload": Joi.object({
        fileId: Joi.string().required(),
        uploadId: Joi.string().required(),
    }),
};
