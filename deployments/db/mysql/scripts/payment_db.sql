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

CREATE TABLE `BatchPayouts` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Date` datetime(6) NOT NULL,
    `Method` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ReferencePaypalPayoutBatchId` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_BatchPayouts` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Courses` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatorId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Price` decimal(65,30) NOT NULL,
    `Status` int NOT NULL,
    CONSTRAINT `PK_Courses` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Payments` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Username` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CourseId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CourseName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CourseCreatorId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `PaymentDueDate` datetime(6) NOT NULL,
    `RefundDueDate` datetime(6) NOT NULL,
    `RefundReason` longtext CHARACTER SET utf8mb4 NULL,
    `ListPrice` decimal(65,30) NOT NULL,
    `Discount` decimal(65,30) NOT NULL,
    `TotalPrice` decimal(65,30) NOT NULL,
    `CouponCode` longtext CHARACTER SET utf8mb4 NULL,
    `Status` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Method` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PaypalOrderId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Payments` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PaypalPayerId` longtext CHARACTER SET utf8mb4 NULL,
    `PaypalEmail` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_BatchPayouts_Date` ON `BatchPayouts` (`Date`);

CREATE INDEX `IX_Payments_CreatedAt_Status` ON `Payments` (`CreatedAt`, `Status`);

CREATE UNIQUE INDEX `IX_Payments_PaypalOrderId` ON `Payments` (`PaypalOrderId`);

CREATE INDEX `IX_Payments_Status_PaymentDueDate` ON `Payments` (`Status`, `PaymentDueDate`);

CREATE INDEX `IX_Payments_UserId` ON `Payments` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241201142403_InitialCreate', '8.0.2');

INSERT INTO `Users` VALUES 
('00543305-2d30-4520-9f5b-f35a58931338', 'Admin 1', 'admin1@gmail.com', NULL, NULL),
('5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Teacher 1', 'teacher1@gmail.com', NULL, NULL);

INSERT INTO `Courses` VALUES 
('V90Dh8ykUsci', 'React - The Complete Guide 2024 (incl. Next.js, Redux)', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 24.99, 0),
('FtDOrVhpmBQ6', 'NestJS Masterclass - NodeJS Framework Backend Development', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('SgOyEZIhNu55', 'Khóa học Figma từ căn bản đến thực chiến', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('t55Iqmyu1i28', 'React Ultimate - React.JS Cơ Bản Từ Z Đến A Cho Beginners', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('CNmV2D9DDAOM', 'Backend RESTFul Server với Node.JS và Express (SQL/MongoDB)', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('AKmV2D9DDAAB', 'Nest.JS Zero - Xây Dựng Backend Node.JS Chuyên Nghiệp', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('N62gvz93Urm1', 'React State Manager - Redux Toolkit, React Query, Redux Saga', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('qqmf9aNnhwKr', 'React Pro Max - Làm Chủ Toàn Diện React.JS Hiện Đại', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('pGfVD4Mi9qDP', 'React Pro TypeScript - Thực Hành Dự Án Portfolio', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('1qSuqjVgjnPw', 'Khóa học NextJS 14-ReactJS-Typescript thực chiến 2024 PRO', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('gGqU8OFcapBH', 'HTML/CSS cho người mới bắt đầu 2023', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('7ajoPlpIzcHP', 'Lập trình iOS với SwiftUI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('lV2UdKKIwNrh', 'iOS & Swift - The Complete iOS App Development Bootcamp', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 39.99, 0),
('9AIiyaKglA0c', 'Flutter & Dart - The Complete Guide [2024 Edition]', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 39.99, 0),
('YHGAVB8xinxd', 'The Complete Flutter Development Bootcamp with Dart', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('0VX5sZVdMNzt', 'The Complete React Native + Hooks Course', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('wK9d3vaoGaxn', 'React Native - The Practical Guide [2024]', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('RbSehgsm6j7d', 'The Complete Android Oreo Developer Course - Build 23 Apps!', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('RFSSSe4opxe0', 'The Complete Android 14 & Kotlin Development Masterclass', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('98MvPU0NomMw', 'iOS 10 & Swift 3: From Beginner to Paid Professional™', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('jhHc5DA7xQi1', 'Flutter Beginner Tutorial - Build own App', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('YWMXcsCJm9UF', 'Figmarketing | Khoá học figma dành cho thiết kế marketing', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('BLSxdyq7fO2i', 'Làm hiệu ứng video với After Effects', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('NCYuHjMuouth', 'Adobe Premiere Pro CC – Essentials Training Course', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('fvo3TDLsytTW', 'The Complete Adobe Premiere Pro Video Editing Bootcamp', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('PmjXnCKrIP5n', 'Learn Adobe Photoshop from Scratch', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('66nQRybTgvjh', 'Trọn Bộ 5 Kỹ Năng Thiết Kế Adobe', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('vuGAitOxcLeJ', 'Photoshop - Thiết kế poster/banner (dành cho người mới)', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('2wKf0QNZuccI', 'Bậc Thầy Canva | Tạo Thiết Kế Chuyên Nghiệp Từ Template', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('zYr9qT0qnPAj', 'Adobe Photoshop Masterclass with Photoshop 2025 + AI Updates', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('3zSjLv3XHIw8', 'The Game Design and AI Master Class Beginner to Expert', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('h8zCDaTjtAKW', 'Visual Effects for Games in Unity - Beginner To Intermediate', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('iv2V4Fw47CEv', 'Learn Professional Pixel Art & Animation for Games', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('2CS8DVmYkTOi', 'Learn Professional 2D Game Graphic Design in Photoshop', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('l6TPKDNVEvjV', 'Learn Blender 3D Modeling for Unity Video Game Development', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('Ec6tzjZbmiVf', 'Canva Master Course 2024 | Learn Canva', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('c8QqjvVqLBaA', 'Học máy (Machine learning) và ứng dụng', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('RkSjoT3FUpGq', 'Machine Learning A-Z: AI, Python & R + ChatGPT Prize [2024]', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('vjHPr6xRKL4O', 'Python for Data Science and Machine Learning Bootcamp', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('bY0VrZ1DH8zq', 'The Data Science Course: Complete Data Science Bootcamp 2024', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('a3t7IYvGrOS5', 'Artificial Intelligence A-Z 2024: Build 7 AI + LLM & ChatGPT', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('gI7CWDgpLPiZ', 'Complete C# Unity Game Developer 2D', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('c3hbNyowqbjL', 'Unreal Engine 5 C++ Developer: Learn C++ & Make Video Games', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('wLHy3ymvkV3U', 'Unreal Engine 5: The Complete Beginner\'s Course', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('cXdQ5iWMnNbl', 'Complete C# Unity Game Developer 3D (Updated To Unity 6)', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('iVdLfGu38cKX', 'The Ultimate Guide to Game Development with Unity (Official)', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('pomtzOtrLOEJ', 'The Complete Digital Marketing Course - 12 Courses in 1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('gEJCMF9IPEHJ', 'Digital Marketing Masterclass: AI & Social Media Marketing', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('5WHMoqOualab', 'Mega Digital Marketing Course A-Z: 32 Courses in 1 + Updates', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0),
('ZZfcnUy0bLZe', 'ChatGPT for Marketing, Content, Social Media, and PR', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('ztWxU70bcLZl', 'Digital Marketing Certification: Master Digital Marketing', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('sf9pJOexbu9j', 'SEO 2024: Complete SEO Training - AI SEO Masterclass', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('lGSmWHsQMSh7', 'The Ultimate SEO Training 2024 + SEO For Wordpress Websites', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 29.99, 0),
('4hwsIvOJUbwQ', 'SEO Training: Get Free Traffic to Your Website With SEO', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('VHpj7x0FUWrI', 'Complete SEO Guide to Ranking Local Business Websites', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 19.99, 0),
('VbP6CV6bmGHZ', 'The Complete SEO Bootcamp 2022', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 0, 0);

COMMIT;
