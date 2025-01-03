import mysql from "mysql2";
import * as bcrypt from "bcrypt";
import Config from "../config";

const connection = mysql.createConnection({
    host: Config.Database.Host,
    port: Config.Database.Port,
    user: Config.Database.User,
    password: Config.Database.Password,
    database: Config.Database.Name,
});

connection.connect(function (err) {
    if (err) {
        console.error("Error connecting to mysql: " + err.message);
    }
});

const query = "INSERT INTO users (id, createdAt, name, email, hashedPassword, emailConfirmed, lockoutEnd, role) VALUES ?";
const users = [
    ['00543305-2d30-4520-9f5b-f35a58931338', '2024-09-13 17:08:10.264855', 'Admin 1', 'admin1@gmail.com', bcrypt.hashSync("11111111", 10), 1, null, 'admin'],
    ['5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-09-13 17:08:10.264855', 'Teacher 1', 'teacher1@gmail.com', bcrypt.hashSync("11111111", 10), 1, null, 'instructor'],
];

connection.query(query, [users], (err) => {
    if (err) {
        throw err;
    }

    console.log("SQL seed completed!");
    connection.end();
});