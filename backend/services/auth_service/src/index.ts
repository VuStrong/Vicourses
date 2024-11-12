import Config from "./config";
import express, { Express } from "express";
import { dataSource } from "./data/data-source";
import router from "./routes";
import * as rabbitmq from "./rabbitmq/client";
import errorHandler from "./middlewares/error-handler.middleware";
import logger from "./logger";

// establish database connection
dataSource
    .initialize()
    .then(() => {
        logger.info("Data Source has been initialized!");
    })
    .catch((err) => {
        logger.error("Error during Data Source initialization:", err);
    })

// Connect Rabbitmq
rabbitmq.connect();

const app: Express = express();
const port = Config.PORT;

app.use(express.json());

app.use("/", router);

app.use(errorHandler);

app.listen(port, () => {
    logger.info(`[Server] [${Config.NODE_ENV}]: Server is running on port ${port}`);
});