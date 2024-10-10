import Config from "./config";
import express, { Express } from "express";
import router from "./routes";
import errorHandler from "./middlewares/error-handler.middleware";
import * as rabbitmq from "./rabbitmq/client"

rabbitmq.connect();

const app: Express = express();
const port = Config.PORT;

app.use(express.json());

app.use("/", router);

app.use(errorHandler);

app.listen(port, () => {
    console.log(`[server] [${Config.NODE_ENV}]: Server is running on port ${port}`);
});