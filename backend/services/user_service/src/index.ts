import Config from "./config";
import express, { Express } from "express";
import { dataSource } from "./data/data-source";
import router from "./routes";
import { connect as rabbitMqConnect } from "./rabbitmq/connection";
import logger from "./logger";
import handleError from "./middlewares/handle-error.middleware";

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
rabbitMqConnect();

const app: Express = express();
const port = Config.Port;

app.use(express.json());

app.use("/", router);

app.use(handleError);

app.listen(port, () => {
    logger.info(`[Server] [${Config.Environment}]: Server is running on port ${port}`);
});