USE `Soft`;

LOCK TABLES  `soft`.`scheduledevents` WRITE;

ALTER TABLE `soft`.`scheduledevents`
ADD COLUMN `Color` INT NOT NULL AFTER `LessonID` DEFAULT 3447003;

UNLOCK TABLES;

LOCK TABLE  `soft`.`eventoccurrences` WRITE;

ALTER TABLE `soft`.`eventoccurrences`
ADD COLUMN `Color` INT NOT NULL AFTER `Storage` DEFAULT 3447003;

UNLOCK TABLES;