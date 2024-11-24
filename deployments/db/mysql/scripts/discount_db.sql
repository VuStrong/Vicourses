CREATE DATABASE IF NOT EXISTS discount_db;

CREATE USER 'discountservice'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON `discount_db`.* TO 'discountservice'@'%';

FLUSH PRIVILEGES;

USE discount_db;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Courses` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreatorId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Price` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_Courses` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Coupons` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreatorId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CourseId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ExpiryDate` datetime(6) NOT NULL,
    `Count` int NOT NULL,
    `Remain` int NOT NULL,
    `Discount` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Coupons` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Coupons_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_Coupons_Code` ON `Coupons` (`Code`);

CREATE INDEX `IX_Coupons_CourseId_ExpiryDate` ON `Coupons` (`CourseId`, `ExpiryDate`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241110063303_InitialCreate', '8.0.2');

INSERT INTO `Courses` (`Id`, `CreatorId`, `Price`)
VALUES ('V90Dh8ykUsci', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 24.99),
('FtDOrVhpmBQ6', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 4.99),
('SgOyEZIhNu55', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0);

COMMIT;

