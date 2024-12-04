CREATE DATABASE IF NOT EXISTS user_db;

CREATE USER 'userservice'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON `user_db`.* TO 'userservice'@'%';

FLUSH PRIVILEGES;

USE user_db;

--
-- Table structure for table `migrations`
--

DROP TABLE IF EXISTS `migrations`;
CREATE TABLE `migrations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `timestamp` bigint NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `migrations`
--

LOCK TABLES `migrations` WRITE;
INSERT INTO `migrations` VALUES (1,1732958187637,'InitialCreate1732958187637');
UNLOCK TABLES;

--
-- Table structure for table `paypal_accounts`
--

DROP TABLE IF EXISTS `paypal_accounts`;
CREATE TABLE `paypal_accounts` (
  `id` varchar(36) NOT NULL,
  `payerId` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `refreshToken` varchar(255) NOT NULL,
  `emailVerified` tinyint NOT NULL,
  `verifiedAccount` tinyint NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` varchar(36) NOT NULL,
  `createdAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `hashedPassword` varchar(255) DEFAULT NULL,
  `emailConfirmed` tinyint NOT NULL DEFAULT '0',
  `emailConfirmationToken` varchar(255) DEFAULT NULL,
  `emailConfirmationTokenExpiryTime` datetime DEFAULT NULL,
  `passwordResetToken` varchar(255) DEFAULT NULL,
  `passwordResetTokenExpiryTime` datetime DEFAULT NULL,
  `lockoutEnd` datetime DEFAULT NULL,
  `role` enum('admin','instructor','student') NOT NULL DEFAULT 'student',
  `thumbnailId` varchar(255) DEFAULT NULL,
  `thumbnailUrl` varchar(255) DEFAULT NULL,
  `headline` varchar(255) DEFAULT NULL,
  `description` text,
  `websiteUrl` varchar(255) DEFAULT NULL,
  `youtubeUrl` varchar(255) DEFAULT NULL,
  `facebookUrl` varchar(255) DEFAULT NULL,
  `linkedInUrl` varchar(255) DEFAULT NULL,
  `enrolledCoursesVisible` tinyint NOT NULL DEFAULT '1',
  `isPublic` tinyint NOT NULL DEFAULT '1',
  `courseTags` varchar(255) NOT NULL DEFAULT '',
  `categoryIds` varchar(255) NOT NULL DEFAULT '',
  `totalEnrollmentCount` int NOT NULL DEFAULT '0',
  `paypalAccountId` varchar(36) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IDX_97672ac88f789774dd47f7c8be` (`email`),
  UNIQUE KEY `REL_2a06e61d5000a2f944396aff68` (`paypalAccountId`),
  KEY `IDX_ace513fa30d485cfd25c11a9e4` (`role`),
  CONSTRAINT `FK_2a06e61d5000a2f944396aff689` FOREIGN KEY (`paypalAccountId`) REFERENCES `paypal_accounts` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
INSERT INTO `users` VALUES 
('00543305-2d30-4520-9f5b-f35a58931338','2024-09-13 17:08:10.264855','Admin 1','admin1@gmail.com','$2b$10$B81Ab0kEpbWzEmENS1jDi.B3DRoLLzwoMOsD0lnhu3Qoo6T0Za53C',1,NULL,NULL,NULL,NULL,NULL,'admin',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,1,'','',0,NULL),
('5a8a8a8c-4663-41b5-9849-81ae7f6726e9','2024-09-13 17:08:10.264855','Teacher 1','teacher1@gmail.com','$2b$10$.QAEn/4ciZBaqxGTxyqSmuB63eiaOysYzz9px8GwZwFRnYnQyzV9C',1,NULL,NULL,NULL,NULL,NULL,'instructor',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,1,'','',0,NULL);
UNLOCK TABLES;

--
-- Table structure for table `refresh_tokens`
--

DROP TABLE IF EXISTS `refresh_tokens`;
CREATE TABLE `refresh_tokens` (
  `userId` varchar(255) NOT NULL,
  `token` varchar(255) NOT NULL,
  `expiryTime` datetime NOT NULL,
  PRIMARY KEY (`userId`,`token`),
  CONSTRAINT `FK_610102b60fea1455310ccd299de` FOREIGN KEY (`userId`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;