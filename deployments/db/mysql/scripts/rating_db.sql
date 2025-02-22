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
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ThumbnailUrl` longtext CHARACTER SET utf8mb4 NULL,
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
    `InstructorId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
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

CREATE INDEX `IX_Ratings_InstructorId_CourseId` ON `Ratings` (`InstructorId`, `CourseId`);

CREATE INDEX `IX_Ratings_UserId` ON `Ratings` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250222091227_InitialCreate', '8.0.2');

INSERT INTO `Users` VALUES 
('00543305-2d30-4520-9f5b-f35a58931338','Admin 1','admin1@gmail.com',NULL),
('5a8a8a8c-4663-41b5-9849-81ae7f6726e9','Teacher 1','teacher1@gmail.com',NULL);

INSERT INTO `Courses` VALUES 
('V90Dh8ykUsci', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'React - The Complete Guide 2024 (incl. Next.js, Redux)', 'https://img-c.udemycdn.com/course/240x135/1362070_b9a1_2.jpg', 0, 0, 0),
('FtDOrVhpmBQ6', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'NestJS Masterclass - NodeJS Framework Backend Development', 'https://img-c.udemycdn.com/course/240x135/6048973_c5b2_17.jpg', 0, 0, 0),
('SgOyEZIhNu55', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Khóa học Figma từ căn bản đến thực chiến', 'https://images.squarespace-cdn.com/content/v1/62460a56418af8236d4f3fee/1678917204941-H4A8KPJQYPNYBHQILYDF/Figma.jpg', 0, 0, 0),
('t55Iqmyu1i28', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'React Ultimate - React.JS Cơ Bản Từ Z Đến A Cho Beginners', 'https://img-c.udemycdn.com/course/240x135/4467252_61d1_3.jpg', 0, 0, 0),
('CNmV2D9DDAOM', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Backend RESTFul Server với Node.JS và Express (SQL/MongoDB)', 'https://caodang.fpt.edu.vn/wp-content/uploads/a-9.png', 0, 0, 0),
('AKmV2D9DDAAB', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Nest.JS Zero - Xây Dựng Backend Node.JS Chuyên Nghiệp', 'https://cdn.intuji.com/2022/09/Nestjs_hero1.png', 0, 0, 0),
('N62gvz93Urm1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'React State Manager - Redux Toolkit, React Query, Redux Saga', 'https://img-c.udemycdn.com/course/240x135/1362070_b9a1_2.jpg', 0, 0, 0),
('qqmf9aNnhwKr', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'React Pro Max - Làm Chủ Toàn Diện React.JS Hiện Đại', 'https://img-c.udemycdn.com/course/240x135/1362070_b9a1_2.jpg', 0, 0, 0),
('pGfVD4Mi9qDP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'React Pro TypeScript - Thực Hành Dự Án Portfolio', 'https://img-c.udemycdn.com/course/240x135/1362070_b9a1_2.jpg', 0, 0, 0),
('1qSuqjVgjnPw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Khóa học NextJS 14-ReactJS-Typescript thực chiến 2024 PRO', 'https://images.prismic.io/turing/652ec31afbd9a45bcec81965_Top_Features_in_Next_js_13_7f9a32190f.webp?auto=format,compress', 0, 0, 0),
('gGqU8OFcapBH', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'HTML/CSS cho người mới bắt đầu 2023', 'https://letdiv.com/wp-content/uploads/2024/04/khoa-hoc-html-css.jpg', 0, 0, 0),
('7ajoPlpIzcHP', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Lập trình iOS với SwiftUI', 'https://img-c.udemycdn.com/course/480x270/2870362_1e87_2.jpg', 0, 0, 0),
('lV2UdKKIwNrh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'iOS & Swift - The Complete iOS App Development Bootcamp', 'https://img-c.udemycdn.com/course/480x270/2870362_1e87_2.jpg', 0, 0, 0),
('9AIiyaKglA0c', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Flutter & Dart - The Complete Guide [2024 Edition]', 'https://img-c.udemycdn.com/course/240x135/1708340_7108_5.jpg', 0, 0, 0),
('YHGAVB8xinxd', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete Flutter Development Bootcamp with Dart', 'https://img-c.udemycdn.com/course/240x135/2259120_305f_6.jpg', 0, 0, 0),
('0VX5sZVdMNzt', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete React Native + Hooks Course', 'https://img-c.udemycdn.com/course/240x135/959700_8bd2_12.jpg', 0, 0, 0),
('wK9d3vaoGaxn', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'React Native - The Practical Guide [2024]', 'https://img-c.udemycdn.com/course/240x135/1436092_2024_4.jpg', 0, 0, 0),
('RbSehgsm6j7d', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete Android Oreo Developer Course - Build 23 Apps!', 'https://img-c.udemycdn.com/course/240x135/1405812_931d_2.jpg', 0, 0, 0),
('RFSSSe4opxe0', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete Android 14 & Kotlin Development Masterclass', 'https://img-c.udemycdn.com/course/240x135/2642574_61bb_2.jpg', 0, 0, 0),
('98MvPU0NomMw', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'iOS 10 & Swift 3: From Beginner to Paid Professional™', 'https://img-c.udemycdn.com/course/240x135/892102_963b.jpg', 0, 0, 0),
('jhHc5DA7xQi1', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Flutter Beginner Tutorial - Build own App', 'https://img-c.udemycdn.com/course/240x135/1967418_c3c0.jpg', 0, 0, 0),
('YWMXcsCJm9UF', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Figmarketing | Khoá học figma dành cho thiết kế marketing', 'https://cdn.sanity.io/images/599r6htc/regionalized/3225dab2b34419e6bc17bf52633ed13b4e86cd6d-3262x1836.jpg?rect=1,0,3260,1836&w=1632&h=919&q=75&fit=max&auto=format', 0, 0, 0),
('BLSxdyq7fO2i', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Làm hiệu ứng video với After Effects', 'https://img-c.udemycdn.com/course/240x135/5243536_dd02.jpg', 0, 0, 0),
('NCYuHjMuouth', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Adobe Premiere Pro CC – Essentials Training Course', 'https://sadesign.vn/pictures/picfullsizes/2024/09/19/cbr1726717547.jpg', 0, 0, 0),
('fvo3TDLsytTW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete Adobe Premiere Pro Video Editing Bootcamp', 'https://learnandexcel.in/wp-content/uploads/2022/03/Premiere-Pro-Thumpnai-2-1024x576.jpg', 0, 0, 0),
('PmjXnCKrIP5n', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Learn Adobe Photoshop from Scratch', 'https://img-c.udemycdn.com/course/240x135/270816_7956_3.jpg', 0, 0, 0),
('66nQRybTgvjh', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Trọn Bộ 5 Kỹ Năng Thiết Kế Adobe', 'https://img-c.udemycdn.com/course/240x135/5499942_3a51_2.jpg', 0, 0, 0),
('vuGAitOxcLeJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Photoshop - Thiết kế poster/banner (dành cho người mới)', 'https://www.classcentral.com/report/wp-content/uploads/2022/05/Adobe-Photoshop-BCG-Banner-1.png', 0, 0, 0),
('2wKf0QNZuccI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Bậc Thầy Canva | Tạo Thiết Kế Chuyên Nghiệp Từ Template', 'https://img-c.udemycdn.com/course/750x422/4992484_8e0b_3.jpg', 0, 0, 0),
('zYr9qT0qnPAj', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Adobe Photoshop Masterclass with Photoshop 2025 + AI Updates', 'https://www.nextscreen.in/blog/wp-content/uploads/2022/04/is-it-possible-to-learn-photoshop-_1.jpg', 0, 0, 0),
('3zSjLv3XHIw8', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Game Design and AI Master Class Beginner to Expert', 'https://img-c.udemycdn.com/course/240x135/526534_04bb_4.jpg', 0, 0, 0),
('h8zCDaTjtAKW', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Visual Effects for Games in Unity - Beginner To Intermediate', 'https://img-c.udemycdn.com/course/240x135/1318732_86ee_3.jpg', 0, 0, 0),
('iv2V4Fw47CEv', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Learn Professional Pixel Art & Animation for Games', 'https://img-c.udemycdn.com/course/240x135/1318732_86ee_3.jpg', 0, 0, 0),
('2CS8DVmYkTOi', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Learn Professional 2D Game Graphic Design in Photoshop', 'https://img-c.udemycdn.com/course/240x135/707876_9e82_4.jpg', 0, 0, 0),
('l6TPKDNVEvjV', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Learn Blender 3D Modeling for Unity Video Game Development', 'https://img-c.udemycdn.com/course/240x135/625846_e25b.jpg', 0, 0, 0),
('Ec6tzjZbmiVf', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Canva Master Course 2024 | Learn Canva', 'https://img-c.udemycdn.com/course/750x422/5308280_ab92_2.jpg', 0, 0, 0),
('c8QqjvVqLBaA', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Học máy (Machine learning) và ứng dụng', 'https://img-c.udemycdn.com/course/240x135/1849356_5a7f_5.jpg', 0, 0, 0),
('RkSjoT3FUpGq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Machine Learning A-Z: AI, Python & R + ChatGPT Prize [2024]', 'https://img-c.udemycdn.com/course/240x135/950390_270f_3.jpg', 0, 0, 0),
('vjHPr6xRKL4O', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Python for Data Science and Machine Learning Bootcamp', 'https://img-c.udemycdn.com/course/240x135/903744_8eb2.jpg', 0, 0, 0),
('bY0VrZ1DH8zq', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Data Science Course: Complete Data Science Bootcamp 2024', 'https://img-c.udemycdn.com/course/240x135/1754098_e0df_3.jpg', 0, 0, 0),
('a3t7IYvGrOS5', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Artificial Intelligence A-Z 2024: Build 7 AI + LLM & ChatGPT', 'https://img-c.udemycdn.com/course/240x135/1219332_bdd7.jpg', 0, 0, 0),
('gI7CWDgpLPiZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Complete C# Unity Game Developer 2D', 'https://img-c.udemycdn.com/course/240x135/258316_55e9_12.jpg', 0, 0, 0),
('c3hbNyowqbjL', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Unreal Engine 5 C++ Developer: Learn C++ & Make Video Games', 'https://img-c.udemycdn.com/course/240x135/657932_c7e0_6.jpg', 0, 0, 0),
('wLHy3ymvkV3U', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Unreal Engine 5: The Complete Beginner\'s Course', 'https://img-c.udemycdn.com/course/240x135/660600_39b2_4.jpg', 0, 0, 0),
('cXdQ5iWMnNbl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Complete C# Unity Game Developer 3D (Updated To Unity 6)', 'https://img-c.udemycdn.com/course/240x135/1178124_76bb_13.jpg', 0, 0, 0),
('iVdLfGu38cKX', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Ultimate Guide to Game Development with Unity (Official)', 'https://img-c.udemycdn.com/course/240x135/1328572_b05d_5.jpg', 0, 0, 0),
('pomtzOtrLOEJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete Digital Marketing Course - 12 Courses in 1', 'https://img-c.udemycdn.com/course/240x135/914296_3670_8.jpg', 0, 0, 0),
('gEJCMF9IPEHJ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Digital Marketing Masterclass: AI & Social Media Marketing', 'https://courses.iid.org.in/public//uploads/media_manager/628.jpg', 0, 0, 0),
('5WHMoqOualab', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Mega Digital Marketing Course A-Z: 32 Courses in 1 + Updates', 'https://img-c.udemycdn.com/course/240x135/1931752_1012_16.jpg', 0, 0, 0),
('ZZfcnUy0bLZe', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'ChatGPT for Marketing, Content, Social Media, and PR', 'https://img-c.udemycdn.com/course/240x135/5127818_abb9_3.jpg', 0, 0, 0),
('ztWxU70bcLZl', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Digital Marketing Certification: Master Digital Marketing', 'https://img-c.udemycdn.com/course/240x135/2658548_c93b_6.jpg', 0, 0, 0),
('sf9pJOexbu9j', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'SEO 2024: Complete SEO Training - AI SEO Masterclass', 'https://www.classcentral.com/report/wp-content/uploads/2022/07/SEO-BCG-Banner-2.png', 0, 0, 0),
('lGSmWHsQMSh7', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Ultimate SEO Training 2024 + SEO For Wordpress Websites', 'https://img-c.udemycdn.com/course/240x135/2938000_f552_23.jpg', 0, 0, 0),
('4hwsIvOJUbwQ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'SEO Training: Get Free Traffic to Your Website With SEO', 'https://img-c.udemycdn.com/course/240x135/365176_1284_12.jpg', 0, 0, 0),
('VHpj7x0FUWrI', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'Complete SEO Guide to Ranking Local Business Websites', 'https://img-c.udemycdn.com/course/240x135/632826_9159.jpg', 0, 0, 0),
('VbP6CV6bmGHZ', '5a8a8a8c-4663-41b5-9849-81ae7f6726e9', 'The Complete SEO Bootcamp 2022', 'https://img-c.udemycdn.com/course/240x135/1433858_a050_2.jpg', 0, 0, 0);

COMMIT;

