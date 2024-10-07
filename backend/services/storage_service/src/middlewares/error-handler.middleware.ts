import { NextFunction, Request, Response } from "express"
import { AppError } from "../utils/app-error";
import { MulterError } from "multer";

export default function errorHandler(err: Error, req: Request, res: Response, next: NextFunction) {
    if (err instanceof AppError) {
		return res.status(err.statusCode).send({
			message: err.message,
			code: err.statusCode,
		});
    }
	if (err instanceof MulterError) {
		return res.status(400).send({
			message: `Field ${err.field}: ${err.message}`,
			code: 400,
		});
	}
	
	res.status(500).send({
		message: "Something went wrong!"
	});
}