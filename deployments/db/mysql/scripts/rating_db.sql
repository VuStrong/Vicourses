CREATE DATABASE IF NOT EXISTS rating_db;

CREATE USER 'ratingservice'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON `rating_db`.* TO 'ratingservice'@'%';

FLUSH PRIVILEGES;

USE rating_db;

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
    `Status` int NOT NULL,
    `AvgRating` decimal(65,30) NOT NULL,
    `RatingCount` int NOT NULL,
    CONSTRAINT `PK_Courses` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Enrollments` (
    `CourseId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Enrollments` PRIMARY KEY (`CourseId`, `UserId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ThumbnailUrl` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Ratings` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CourseId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Feedback` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Star` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `Responded` tinyint(1) NOT NULL,
    `Response` longtext CHARACTER SET utf8mb4 NULL,
    `RespondedAt` datetime(6) NULL,
    CONSTRAINT `PK_Ratings` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Ratings_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Ratings_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_Ratings_CourseId_UserId` ON `Ratings` (`CourseId`, `UserId`);

CREATE INDEX `IX_Ratings_UserId` ON `Ratings` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241105152014_InitialCreate', '8.0.2');

INSERT INTO `Users` VALUES 
('00543305-2d30-4520-9f5b-f35a58931338','Admin 1','admin1@gmail.com',NULL),
('5a8a8a8c-4663-41b5-9849-81ae7f6726e9','Teacher 1','teacher1@gmail.com',NULL);

INSERT INTO `Courses` VALUES 
('V90Dh8ykUsci', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('FtDOrVhpmBQ6', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('SgOyEZIhNu55', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('t55Iqmyu1i28', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('CNmV2D9DDAOM', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('AKmV2D9DDAAB', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('N62gvz93Urm1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('qqmf9aNnhwKr', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('pGfVD4Mi9qDP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('1qSuqjVgjnPw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('gGqU8OFcapBH', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('7ajoPlpIzcHP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('lV2UdKKIwNrh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('9AIiyaKglA0c', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('YHGAVB8xinxd', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('0VX5sZVdMNzt', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('wK9d3vaoGaxn', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('RbSehgsm6j7d', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('RFSSSe4opxe0', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('98MvPU0NomMw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('jhHc5DA7xQi1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('YWMXcsCJm9UF', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('BLSxdyq7fO2i', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('NCYuHjMuouth', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('fvo3TDLsytTW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('PmjXnCKrIP5n', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('66nQRybTgvjh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('vuGAitOxcLeJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('2wKf0QNZuccI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('zYr9qT0qnPAj', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('3zSjLv3XHIw8', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('h8zCDaTjtAKW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('iv2V4Fw47CEv', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('2CS8DVmYkTOi', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('l6TPKDNVEvjV', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('Ec6tzjZbmiVf', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('c8QqjvVqLBaA', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('RkSjoT3FUpGq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('vjHPr6xRKL4O', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('bY0VrZ1DH8zq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('a3t7IYvGrOS5', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('gI7CWDgpLPiZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('c3hbNyowqbjL', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('wLHy3ymvkV3U', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('cXdQ5iWMnNbl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('iVdLfGu38cKX', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('pomtzOtrLOEJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('gEJCMF9IPEHJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('5WHMoqOualab', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('ZZfcnUy0bLZe', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('ztWxU70bcLZl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('sf9pJOexbu9j', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('lGSmWHsQMSh7', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('4hwsIvOJUbwQ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('VHpj7x0FUWrI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0),
('VbP6CV6bmGHZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0);

COMMIT;

