CREATE DATABASE IF NOT EXISTS payment_db;

CREATE USER 'paymentservice'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON `payment_db`.* TO 'paymentservice'@'%';

FLUSH PRIVILEGES;

USE payment_db;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Payments` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Username` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CourseId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CourseName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `PaymentDueDate` datetime(6) NOT NULL,
    `ListPrice` decimal(65,30) NOT NULL,
    `Discount` decimal(65,30) NOT NULL,
    `TotalPrice` decimal(65,30) NOT NULL,
    `CouponCode` longtext CHARACTER SET utf8mb4 NULL,
    `Status` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Method` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PaypalOrderId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Payments` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_Payments_PaypalOrderId` ON `Payments` (`PaypalOrderId`);

CREATE INDEX `IX_Payments_Status_PaymentDueDate` ON `Payments` (`Status`, `PaymentDueDate`);

CREATE INDEX `IX_Payments_UserId` ON `Payments` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241124080714_InitialCreate', '8.0.2');

COMMIT;

