import winston from "winston";
import {
    ElasticsearchTransport,
    ElasticsearchTransportOptions,
} from "winston-elasticsearch";
import Config from "../config";
import * as fs from 'fs';

const esTransportOpts: ElasticsearchTransportOptions = {
    indexPrefix: "vicourses-services-logs",
    clientOpts: {
        node: Config.ELASTICSEARCH_URL,
        auth: {
            username: Config.ELASTICSEARCH_USER || "",
            password: Config.ELASTICSEARCH_PASS || "",
        },
        tls: {
            ca: fs.readFileSync(Config.ELASTICSEARCH_CERTS_PATH || "")
        }
    },
};

const esTransport = new ElasticsearchTransport(esTransportOpts);
const logger = winston.createLogger({
    format: winston.format.combine(
        winston.format.label({ label: "Auth Service", message: true }),
        winston.format.prettyPrint()
    ),
    transports: [new winston.transports.Console(), esTransport],
});

export default logger;
