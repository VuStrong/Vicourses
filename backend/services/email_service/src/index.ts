import { createServer } from "node:http";
import Config from "./config";
import * as rabbitmq from "./rabbitmq/client";
import { handleHealthChecks } from "./controllers/healthchecks.controller";
import logger from "./logger";

rabbitmq.connectAndConsume();

const port = Config.Port;

const server = createServer(async (req, res) => {
    const reqUrl = req.url;
    const method = req.method;

    if (method === "GET" && reqUrl === "/hc") {
        return await handleHealthChecks(req, res);
    }

    res.writeHead(200, {
        "Content-Type": "application/json",
    });
    res.write(JSON.stringify({
        message: "Email Service is running"
    }));
    res.end();
});

server.listen(port, () => {
    logger.info(`Server is running on Port ${port}`);
});
