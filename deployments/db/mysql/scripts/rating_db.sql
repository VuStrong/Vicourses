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
('SgOyEZIhNu55', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0, 0);

COMMIT;

