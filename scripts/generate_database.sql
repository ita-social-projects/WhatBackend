-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema soft
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `soft` ;

-- -----------------------------------------------------
-- Schema soft
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `soft` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `soft` ;

-- -----------------------------------------------------
-- Table `soft`.`account`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`account` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `role` TINYINT(4) NULL DEFAULT NULL COMMENT 'from enum of roles:\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\n            1 - student\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\n            2 - mentor\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\n            4 - admin',
  `first_name` VARCHAR(30) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL,
  `last_name` VARCHAR(30) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL,
  `email` VARCHAR(50) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'email has been set to not null and unique',
  `password` VARCHAR(65) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'password has been set to not null',
  `salt` VARCHAR(65) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'salt has been set to not null',
  `is_active` TINYINT(1) NOT NULL DEFAULT '1' COMMENT 'is_active has been set to not null with true as a default value',
  `forgot_password_token` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL COMMENT 'token for resetting password',
  `forgot_token_gen_date` DATETIME NULL DEFAULT NULL COMMENT 'date of generation for users forgot password token',
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE` (`email` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`course`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`course` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'name has been set to not null and unique',
  `is_active` TINYINT(1) NOT NULL DEFAULT '1' COMMENT 'is_active has been set to not null with true as a default value', 
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`mentor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`mentor` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `account_id` BIGINT(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_account_of_mentor` (`account_id` ASC),
  CONSTRAINT `FK_account_of_mentor`
    FOREIGN KEY (`account_id`)
    REFERENCES `soft`.`account` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`student_group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`student_group` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `course_id` BIGINT(20) NULL DEFAULT NULL,
  `name` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'name has been set to not null and unique',
  `start_date` DATE NULL DEFAULT NULL,
  `finish_date` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC),
  INDEX `FK_course_of_student_group` (`course_id` ASC),
  CONSTRAINT `FK_course_of_student_group`
    FOREIGN KEY (`course_id`)
    REFERENCES `soft`.`course` (`id`)
    ON DELETE SET NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`theme`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`theme` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'name has been set to not null and unique',
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`lesson`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`lesson` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `mentor_id` BIGINT(20) NULL DEFAULT NULL,
  `student_group_id` BIGINT(20) NULL DEFAULT NULL,
  `theme_id` BIGINT(20) NULL DEFAULT NULL,
  `lesson_date` DATETIME NOT NULL COMMENT 'lesson_date has been set to not null',
  PRIMARY KEY (`id`),
  INDEX `FK_mentor_of_lesson` (`mentor_id` ASC),
  INDEX `FK_student_group_of_lesson` (`student_group_id` ASC),
  INDEX `FK_ThemeOfLesson_idx` (`theme_id` ASC),
  CONSTRAINT `FK_mentor_of_lesson`
    FOREIGN KEY (`mentor_id`)
    REFERENCES `soft`.`mentor` (`id`),
  CONSTRAINT `FK_student_group_of_lesson`
    FOREIGN KEY (`student_group_id`)
    REFERENCES `soft`.`student_group` (`id`),
  CONSTRAINT `FK_theme_of_lesson`
    FOREIGN KEY (`theme_id`)
    REFERENCES `soft`.`theme` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`mentor_of_course`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`mentor_of_course` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `course_id` BIGINT(20) NULL DEFAULT NULL,
  `mentor_id` BIGINT(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `mentorAndCourseIndex` (`course_id` ASC, `mentor_id` ASC),
  INDEX `FK_mentorId` (`mentor_id` ASC),
  CONSTRAINT `FK_course_of_mentor`
    FOREIGN KEY (`course_id`)
    REFERENCES `soft`.`course` (`id`),
  CONSTRAINT `FK_mentor_of_course`
    FOREIGN KEY (`mentor_id`)
    REFERENCES `soft`.`mentor` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`mentor_of_student_group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`mentor_of_student_group` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `mentor_id` BIGINT(20) NULL DEFAULT NULL,
  `student_group_id` BIGINT(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `mentorAndStudentGroupIndex` (`mentor_id` ASC, `student_group_id` ASC),
  INDEX `FK__idx` (`student_group_id` ASC),
  CONSTRAINT `FK_mentor_of_student_group`
    FOREIGN KEY (`mentor_id`)
    REFERENCES `soft`.`mentor` (`id`),
  CONSTRAINT `FK_student_group_of_mentor`
    FOREIGN KEY (`student_group_id`)
    REFERENCES `soft`.`student_group` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`secretary`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`secretary` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `account_id` BIGINT(20) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_account_of_secretary` (`account_id` ASC),
  CONSTRAINT `FK_account_of_secretary`
    FOREIGN KEY (`account_id`)
    REFERENCES `soft`.`account` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`schedule`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`schedule` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `student_group_id` BIGINT(20) NOT NULL,
  `lesson_start` TIME NOT NULL,
  `lesson_end` TIME NOT NULL,
  `repeat_rate` int NOT NULL,
  `day_number` INT UNSIGNED NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_student_group_of_schedule` (`student_group_id` ASC),
  CONSTRAINT `FK_student_group_of_schedule`
    FOREIGN KEY (`student_group_id`)
    REFERENCES `soft`.`student_group` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`student`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`student` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `account_id` BIGINT(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_account_of_student` (`account_id` ASC),
  CONSTRAINT `FK_account_of_student`
    FOREIGN KEY (`account_id`)
    REFERENCES `soft`.`account` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`student_of_student_group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`student_of_student_group` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `student_group_id` BIGINT(20) NULL DEFAULT NULL,
  `student_id` BIGINT(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK__idx` (`student_group_id` ASC),
  INDEX `FK_student_of_student_group_idx` (`student_id` ASC),
  CONSTRAINT `FK_student_group_of_student`
    FOREIGN KEY (`student_group_id`)
    REFERENCES `soft`.`student_group` (`id`),
  CONSTRAINT `FK_student_of_student_group`
    FOREIGN KEY (`student_id`)
    REFERENCES `soft`.`student` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `soft`.`visit`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`visit` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `student_id` BIGINT(20) NULL DEFAULT NULL,
  `lesson_id` BIGINT(20) NULL DEFAULT NULL,
  `student_mark` TINYINT(4) NULL DEFAULT NULL,
  `presence` TINYINT(1) NOT NULL COMMENT 'presence default value has been set',
  `comment` VARCHAR(1024) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `FK_lesson_of_visit` (`lesson_id` ASC),
  INDEX `FK_student_of_visit_idx` (`student_id` ASC),
  CONSTRAINT `FK_lesson_of_visit`
    FOREIGN KEY (`lesson_id`)
    REFERENCES `soft`.`lesson` (`id`),
  CONSTRAINT `FK_student_of_visit`
    FOREIGN KEY (`student_id`)
    REFERENCES `soft`.`student` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `soft`.`attachment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `soft`.`attachment` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `created_on` DATETIME NOT NULL COMMENT 'created_on has been set to not null',
  `created_by_account_id` BIGINT(20) NOT NULL,
  `container_name` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'container_name has been set to not null and unique',
  `file_name` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL COMMENT 'file_name has been set to not null',
  PRIMARY KEY (`id`),
  UNIQUE INDEX `container_name_UNIQUE` (`container_name` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
