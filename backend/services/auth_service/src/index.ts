import Config from "./config";
import express, { Express } from "express";
import { dataSource } from "./data/data-source";
import router from "./routes";
import * as rabbitmq from "./rabbitmq/client";
import errorHandler from "./middlewares/error-handler.middleware";

// establish database connection
dataSource
    .initialize()
    .then(() => {
        console.log("Data Source has been initialized!")
    })
    .catch((err) => {
        console.error("Error during Data Source initialization:", err)
    })

// Connect Rabbitmq
rabbitmq.connect();

const app: Express = express();
const port = Config.PORT;

app.use(express.json());

app.use("/", router);

app.use(errorHandler);

app.listen(port, () => {
  console.log(`[server] [${Config.NODE_ENV}]: Server is running on port ${port}`);
});