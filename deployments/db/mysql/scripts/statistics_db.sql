CREATE DATABASE IF NOT EXISTS statistics_db;

CREATE USER 'statisticsservice'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON `statistics_db`.* TO 'statisticsservice'@'%';

FLUSH PRIVILEGES;

USE statistics_db;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Courses` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `InstructorId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PublishedAt` date NOT NULL,
    `Status` int NOT NULL,
    CONSTRAINT `PK_Courses` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `InstructorMetrics` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `InstructorId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CourseId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Date` date NOT NULL,
    `EnrollmentCount` int NOT NULL,
    `Revenue` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_InstructorMetrics` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` date NOT NULL,
    `Role` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Courses_PublishedAt` ON `Courses` (`PublishedAt`);

CREATE UNIQUE INDEX `IX_InstructorMetrics_InstructorId_CourseId_Date` ON `InstructorMetrics` (`InstructorId`, `CourseId`, `Date`);

CREATE INDEX `IX_InstructorMetrics_InstructorId_Date` ON `InstructorMetrics` (`InstructorId`, `Date`);

CREATE INDEX `IX_Users_CreatedAt` ON `Users` (`CreatedAt`);

CREATE INDEX `IX_Users_Role` ON `Users` (`Role`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241117082607_InitialCreate', '8.0.2');

COMMIT;

