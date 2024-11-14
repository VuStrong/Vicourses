CREATE DATABASE IF NOT EXISTS auth_db;

CREATE USER 'authservice'@'%' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON `auth_db`.* TO 'authservice'@'%';

FLUSH PRIVILEGES;

USE auth_db;

DROP TABLE IF EXISTS `migrations`;
CREATE TABLE `migrations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `timestamp` bigint NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


LOCK TABLES `migrations` WRITE;
INSERT INTO `migrations` VALUES (1,1712313191655,'Init1712313191655'),(2,1726655950740,'TokenRefactoring1726655950740');
UNLOCK TABLES;

--
-- Table structure for table `tokens`
--

DROP TABLE IF EXISTS `tokens`;
CREATE TABLE `tokens` (
  `token` varchar(255) NOT NULL,
  `userId` varchar(255) NOT NULL,
  `type` enum('refresh_token','comfirm_email','reset_password') NOT NULL,
  `expiryTime` datetime NOT NULL,
  PRIMARY KEY (`token`,`userId`),
  KEY `IDX_d417e5d35f2434afc4bd48cb4d` (`userId`)
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
  `passwordHash` varchar(255) NOT NULL,
  `emailConfirmed` tinyint NOT NULL DEFAULT '0',
  `locked` tinyint NOT NULL DEFAULT '0',
  `role` enum('admin','instructor','student') NOT NULL DEFAULT 'student',
  PRIMARY KEY (`id`),
  UNIQUE KEY `IDX_97672ac88f789774dd47f7c8be` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
