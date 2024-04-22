import { NextFunction, Request, Response } from "express"
import { AppError } from "../utils/app-error";

export default function errorHandler(err: Error, req: Request, res: Response, next: NextFunction) {
    if (err instanceof AppError) {
		return res.status(err.statusCode).send({
			message: err.message
		});
    }

	res.status(500).send({
		message: "Something went wrong!"
	});
}