import { NextFunction, Request, Response } from "express"
import { AppError } from "../utils/app-error";
import logger from "../logger";

export default function handleError(err: Error, req: Request, res: Response, next: NextFunction) {
    if (err instanceof AppError) {
		return res.status(err.statusCode).send({
			message: err.message,
			code: err.statusCode,
			errors: err.errors.length > 0 ? err.errors : undefined,
		});
    }

	logger.error(`An error has occured: ${err.message}`);

	res.status(500).send({
		message: "Something went wrong!",
        code: 500,
	});
}