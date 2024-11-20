import Joi from "joi";

export const confirmEmailSchema = Joi.object({
    username: Joi.string().required(),
    link: Joi.string().required(),
}).required();

export const resetPasswordEmailSchema = Joi.object({
    username: Joi.string().required(),
    link: Joi.string().required(),
}).required();

export const courseApprovedEmailSchema = Joi.object({
    username: Joi.string().required(),
    courseId: Joi.string().required(),
    courseName: Joi.string().required(),
}).required();

export const courseNotApprovedEmailSchema = Joi.object({
    username: Joi.string().required(),
    courseId: Joi.string().required(),
    courseName: Joi.string().required(),
    reasons: Joi.array().items(Joi.string()).optional(),
}).required();
