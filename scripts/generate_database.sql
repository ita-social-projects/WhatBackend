CREATE DATABASE  IF NOT EXISTS `soft` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `soft`;
-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: soft
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `role` tinyint DEFAULT NULL COMMENT 'from enum of roles:\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\n            1 - student\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\n            2 - mentor\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\n            4 - admin',
  `first_name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `last_name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'email has been set to not null and unique',
  `password` varchar(65) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'password has been set to not null',
  `salt` varchar(65) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'salt has been set to not null',
  `is_active` tinyint(1) NOT NULL DEFAULT '1' COMMENT 'is_active has been set to not null with true as a default value',
  `forgot_password_token` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'token for resetting password',
  `forgot_token_gen_date` datetime DEFAULT NULL COMMENT 'date of generation for users forgot password token',
  `avatar_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_account_Attachment_avatar_id` FOREIGN KEY (`avatar_id`) REFERENCES `attachment` (`id`) ON DELETE RESTRICT,
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=76 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `attachment`
--

DROP TABLE IF EXISTS `attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachment` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `created_on` datetime NOT NULL COMMENT 'created_on has been set to not null',
  `created_by_account_id` bigint NOT NULL,
  `container_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'container_name has been set to not null and unique',
  `file_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'file_name has been set to not null',
  PRIMARY KEY (`id`),
  UNIQUE KEY `container_name_UNIQUE` (`container_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `attachment_of_homework`
--

DROP TABLE IF EXISTS `attachment_of_homework`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachment_of_homework` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `homework_id` bigint NOT NULL,
  `attachment_id` bigint NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_homework_id` (`homework_id`),
  KEY `FK_attachment_id` (`attachment_id`),
  CONSTRAINT `FK_attachment_of_homework` FOREIGN KEY (`attachment_id`) REFERENCES `attachment` (`id`),
  CONSTRAINT `FK_homework_of_attachment` FOREIGN KEY (`homework_id`) REFERENCES `homework` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `course`
--

DROP TABLE IF EXISTS `course`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `course` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'name has been set to not null and unique',
  `is_active` tinyint(1) NOT NULL DEFAULT '1' COMMENT 'is_active has been set to not null with true as a default value',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event_occurence`
--

DROP TABLE IF EXISTS `event_occurence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `event_occurence` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `student_group_id` bigint NOT NULL,
  `event_start` datetime NOT NULL,
  `event_finish` datetime NOT NULL,
  `pattern` int NOT NULL,
  `storage` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_student_group_of_schedule` (`student_group_id`),
  CONSTRAINT `FK_student_group_of_schedule` FOREIGN KEY (`student_group_id`) REFERENCES `student_group` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `homework`
--

DROP TABLE IF EXISTS `homework`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE IF NOT EXISTS `soft`.`homework` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `due_date` DATETIME NULL DEFAULT NULL,
  `task_text` TEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL,
  `lesson_id` BIGINT(20) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_lesson_of_homework` (`lesson_id` ASC),
  CONSTRAINT `FK_lesson_of_homework`
    FOREIGN KEY (`lesson_id`)
    REFERENCES `soft`.`lesson` (`id`))
 ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lesson`
--

DROP TABLE IF EXISTS `lesson`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lesson` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `mentor_id` bigint DEFAULT NULL,
  `student_group_id` bigint DEFAULT NULL,
  `theme_id` bigint DEFAULT NULL,
  `lesson_date` datetime NOT NULL COMMENT 'lesson_date has been set to not null',
  PRIMARY KEY (`id`),
  KEY `FK_mentor_of_lesson` (`mentor_id`),
  KEY `FK_student_group_of_lesson` (`student_group_id`),
  KEY `FK_ThemeOfLesson_idx` (`theme_id`),
  CONSTRAINT `FK_mentor_of_lesson` FOREIGN KEY (`mentor_id`) REFERENCES `mentor` (`id`),
  CONSTRAINT `FK_student_group_of_lesson` FOREIGN KEY (`student_group_id`) REFERENCES `student_group` (`id`),
  CONSTRAINT `FK_theme_of_lesson` FOREIGN KEY (`theme_id`) REFERENCES `theme` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mentor`
--

DROP TABLE IF EXISTS `mentor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mentor` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `account_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_account_of_mentor` (`account_id`),
  CONSTRAINT `FK_account_of_mentor` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mentor_of_course`
--

DROP TABLE IF EXISTS `mentor_of_course`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mentor_of_course` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `course_id` bigint DEFAULT NULL,
  `mentor_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `mentorAndCourseIndex` (`course_id`,`mentor_id`),
  KEY `FK_mentorId` (`mentor_id`),
  CONSTRAINT `FK_course_of_mentor` FOREIGN KEY (`course_id`) REFERENCES `course` (`id`),
  CONSTRAINT `FK_mentor_of_course` FOREIGN KEY (`mentor_id`) REFERENCES `mentor` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mentor_of_student_group`
--

DROP TABLE IF EXISTS `mentor_of_student_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mentor_of_student_group` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `mentor_id` bigint DEFAULT NULL,
  `student_group_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `mentorAndStudentGroupIndex` (`mentor_id`,`student_group_id`),
  KEY `FK__idx` (`student_group_id`),
  CONSTRAINT `FK_mentor_of_student_group` FOREIGN KEY (`mentor_id`) REFERENCES `mentor` (`id`),
  CONSTRAINT `FK_student_group_of_mentor` FOREIGN KEY (`student_group_id`) REFERENCES `student_group` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=216 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `scheduled_event`
--

DROP TABLE IF EXISTS `scheduled_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheduled_event` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `event_occurence_id` bigint NOT NULL,
  `student_group_id` bigint DEFAULT NULL,
  `theme_id` bigint DEFAULT NULL,
  `mentor_id` bigint DEFAULT NULL,
  `lesson_id` bigint DEFAULT NULL UNIQUE,
  `event_start` datetime NOT NULL,
  `event_finish` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_scheduled_events_event_occurence1_idx` (`event_occurence_id`),
  KEY `fk_scheduled_events_student_group1_idx` (`student_group_id`),
  KEY `fk_scheduled_events_theme1_idx` (`theme_id`),
  KEY `fk_scheduled_events_mentor1_idx` (`mentor_id`),
  KEY `fk_scheduled_events_lesson1_idx` (`lesson_id`),
  CONSTRAINT `fk_scheduled_events_event_occurence1` FOREIGN KEY (`event_occurence_id`) REFERENCES `event_occurence` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_scheduled_events_lesson1` FOREIGN KEY (`lesson_id`) REFERENCES `lesson` (`id`),
  CONSTRAINT `fk_scheduled_events_mentor1` FOREIGN KEY (`mentor_id`) REFERENCES `mentor` (`id`),
  CONSTRAINT `fk_scheduled_events_student_group1` FOREIGN KEY (`student_group_id`) REFERENCES `student_group` (`id`),
  CONSTRAINT `fk_scheduled_events_theme1` FOREIGN KEY (`theme_id`) REFERENCES `theme` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `secretary`
--

DROP TABLE IF EXISTS `secretary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `secretary` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `account_id` bigint NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_account_of_secretary` (`account_id`),
  CONSTRAINT `FK_account_of_secretary` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `account_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_account_of_student` (`account_id`),
  CONSTRAINT `FK_account_of_student` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student_group`
--

DROP TABLE IF EXISTS `student_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student_group` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `course_id` bigint DEFAULT NULL,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'name has been set to not null and unique',
  `start_date` date DEFAULT NULL,
  `finish_date` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  KEY `FK_course_of_student_group` (`course_id`),
  CONSTRAINT `FK_course_of_student_group` FOREIGN KEY (`course_id`) REFERENCES `course` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student_of_student_group`
--

DROP TABLE IF EXISTS `student_of_student_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student_of_student_group` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `student_group_id` bigint DEFAULT NULL,
  `student_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK__idx` (`student_group_id`),
  KEY `FK_student_of_student_group_idx` (`student_id`),
  CONSTRAINT `FK_student_group_of_student` FOREIGN KEY (`student_group_id`) REFERENCES `student_group` (`id`),
  CONSTRAINT `FK_student_of_student_group` FOREIGN KEY (`student_id`) REFERENCES `student` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=496 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `theme`
--

DROP TABLE IF EXISTS `theme`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `theme` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'name has been set to not null and unique',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `visit`
--

DROP TABLE IF EXISTS `visit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `visit` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `student_id` bigint DEFAULT NULL,
  `lesson_id` bigint DEFAULT NULL,
  `student_mark` tinyint DEFAULT NULL,
  `presence` tinyint(1) NOT NULL COMMENT 'presence default value has been set',
  `comment` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_lesson_of_visit` (`lesson_id`),
  KEY `FK_student_of_visit_idx` (`student_id`),
  CONSTRAINT `FK_lesson_of_visit` FOREIGN KEY (`lesson_id`) REFERENCES `lesson` (`id`),
  CONSTRAINT `FK_student_of_visit` FOREIGN KEY (`student_id`) REFERENCES `student` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=400 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

-- -----------------------------------------------------
-- Table `soft`.`homework_from_student`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`homework_from_student` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `student_id` bigint(20) not null,
  `homework_text` TEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL,
  `homework_id` BIGINT(20) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_student_homework` (`homework_id` ASC),
  CONSTRAINT `FK_student_homework`
    FOREIGN KEY (`homework_id`)
    REFERENCES `soft`.`homework` (`id`),
  CONSTRAINT `FK_homework_of_student`
    FOREIGN KEY (`student_id`)
    REFERENCES `soft`.`student` (`id`))

ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
-- -----------------------------------------------------
-- Table `soft`.`attachment_of_homework`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`attachment_of_homework_student` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `attachment_id` BIGINT(20) NOT NULL,
  `homework_student_id` BIGINT(20) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_homework_student_of_attachment_id` (`homework_student_id` ASC),
  INDEX `FK_attachment_of_homework_student_id` (`attachment_id` ASC),
  CONSTRAINT `FK_homework_student_of_attachment_id`
    FOREIGN KEY (`homework_student_id`)
    REFERENCES `soft`.`homework_from_student` (`id`),
  CONSTRAINT `"FK_attachment_of_homework_student_id"`
    FOREIGN KEY (`attachment_id`)
    REFERENCES `soft`.`attachment` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

--
-- Dumping events for database 'soft'
--

--
-- Dumping routines for database 'soft'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-01-26 14:27:34
