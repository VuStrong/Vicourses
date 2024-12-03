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

CREATE TABLE `AdminMetrics` (
    `Date` date NOT NULL,
    `Revenue` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_AdminMetrics` PRIMARY KEY (`Date`)
) CHARACTER SET=utf8mb4;

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
    `RefundCount` int NOT NULL,
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
VALUES ('20241203115221_InitialCreate', '8.0.2');

INSERT INTO `Users` VALUES 
('00543305-2d30-4520-9f5b-f35a58931338','2024-11-20','admin'),
('5a8a8a8c-4663-41b5-9849-81ae7f6726e9','2024-11-20','instructor');

INSERT INTO `Courses` VALUES 
('V90Dh8ykUsci', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('FtDOrVhpmBQ6', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('SgOyEZIhNu55', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('t55Iqmyu1i28', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('CNmV2D9DDAOM', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('AKmV2D9DDAAB', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('N62gvz93Urm1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('qqmf9aNnhwKr', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('pGfVD4Mi9qDP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('1qSuqjVgjnPw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('gGqU8OFcapBH', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('7ajoPlpIzcHP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('lV2UdKKIwNrh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('9AIiyaKglA0c', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('YHGAVB8xinxd', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('0VX5sZVdMNzt', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('wK9d3vaoGaxn', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('RbSehgsm6j7d', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('RFSSSe4opxe0', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('98MvPU0NomMw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('jhHc5DA7xQi1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('YWMXcsCJm9UF', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('BLSxdyq7fO2i', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('NCYuHjMuouth', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('fvo3TDLsytTW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('PmjXnCKrIP5n', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('66nQRybTgvjh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('vuGAitOxcLeJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('2wKf0QNZuccI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('zYr9qT0qnPAj', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('3zSjLv3XHIw8', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('h8zCDaTjtAKW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('iv2V4Fw47CEv', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('2CS8DVmYkTOi', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('l6TPKDNVEvjV', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('Ec6tzjZbmiVf', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('c8QqjvVqLBaA', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('RkSjoT3FUpGq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('vjHPr6xRKL4O', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('bY0VrZ1DH8zq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('a3t7IYvGrOS5', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('gI7CWDgpLPiZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('c3hbNyowqbjL', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('wLHy3ymvkV3U', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('cXdQ5iWMnNbl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('iVdLfGu38cKX', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('pomtzOtrLOEJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('gEJCMF9IPEHJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('5WHMoqOualab', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('ZZfcnUy0bLZe', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('ztWxU70bcLZl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('sf9pJOexbu9j', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('lGSmWHsQMSh7', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('4hwsIvOJUbwQ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('VHpj7x0FUWrI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0),
('VbP6CV6bmGHZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', '2024-11-20', 0);


COMMIT;

