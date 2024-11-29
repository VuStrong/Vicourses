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

INSERT INTO `Courses` (`Id`, `CreatorId`, `Price`) VALUES 
('V90Dh8ykUsci', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 24.99),
('FtDOrVhpmBQ6', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('SgOyEZIhNu55', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('t55Iqmyu1i28', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('CNmV2D9DDAOM', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('AKmV2D9DDAAB', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('N62gvz93Urm1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('qqmf9aNnhwKr', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('pGfVD4Mi9qDP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('1qSuqjVgjnPw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('gGqU8OFcapBH', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('7ajoPlpIzcHP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('lV2UdKKIwNrh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 39.99),
('9AIiyaKglA0c', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 39.99),
('YHGAVB8xinxd', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('0VX5sZVdMNzt', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('wK9d3vaoGaxn', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('RbSehgsm6j7d', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('RFSSSe4opxe0', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('98MvPU0NomMw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('jhHc5DA7xQi1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('YWMXcsCJm9UF', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('BLSxdyq7fO2i', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('NCYuHjMuouth', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('fvo3TDLsytTW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('PmjXnCKrIP5n', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('66nQRybTgvjh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('vuGAitOxcLeJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('2wKf0QNZuccI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('zYr9qT0qnPAj', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('3zSjLv3XHIw8', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('h8zCDaTjtAKW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('iv2V4Fw47CEv', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('2CS8DVmYkTOi', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('l6TPKDNVEvjV', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('Ec6tzjZbmiVf', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('c8QqjvVqLBaA', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('RkSjoT3FUpGq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('vjHPr6xRKL4O', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('bY0VrZ1DH8zq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('a3t7IYvGrOS5', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('gI7CWDgpLPiZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('c3hbNyowqbjL', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('wLHy3ymvkV3U', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('cXdQ5iWMnNbl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('iVdLfGu38cKX', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('pomtzOtrLOEJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('gEJCMF9IPEHJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('5WHMoqOualab', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0),
('ZZfcnUy0bLZe', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('ztWxU70bcLZl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('sf9pJOexbu9j', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('lGSmWHsQMSh7', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99),
('4hwsIvOJUbwQ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('VHpj7x0FUWrI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99),
('VbP6CV6bmGHZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0);

COMMIT;

