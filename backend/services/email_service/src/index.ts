import { createServer } from "node:http";
import Config from "./config";
import * as rabbitmq from "./rabbitmq/client";
import { handleHealthChecks } from "./controllers/healthchecks.controller";
import logger from "./logger";
import { sendEmail } from "./services/email.service";

rabbitmq.connectAndConsume();

const port = Config.PORT;

const server = createServer(async (req, res) => {
    const reqUrl = req.url;
    const method = req.method;

    if (method === "GET" && reqUrl === "/hc") {
        return await handleHealthChecks(req, res);
    }

    sendEmail({
        to: "vubamanh05@gmail.com",
        template: "payment_completed",
        payload: {
            username: "Vu Manh",
            email: "vumanh@gmail.com",
            courseId: "asdassad",
            courseName: "Game Development/Art - Create a 2D Action Game with Unity/C#",
            createdAt: new Date().toISOString(),
            listPrice: 24.99,
            discount: 5.99,
            totalPrice: 19.99,
        }
    });

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
