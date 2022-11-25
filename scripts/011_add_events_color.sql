USE `Soft`;

LOCK TABLES  `soft`.`scheduledevents` WRITE;

ALTER TABLE `soft`.`scheduledevents`
ADD `Color` INT NOT NULL DEFAULT 3447003  AFTER `LessonID`;				

UNLOCK TABLES;

LOCK TABLES  `soft`.`eventoccurrences` WRITE;

ALTER TABLE `soft`.`eventoccurrences`
ADD `Color` INT NOT NULL DEFAULT 3447003 AFTER `Storage`;

UNLOCK TABLES;
