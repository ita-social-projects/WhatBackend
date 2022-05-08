USE `Soft`;

ALTER TABLE `soft`.`scheduledevents`
DROP FOREIGN KEY `FK_EventOccurrenceScheduledEvents`;

ALTER TABLE `soft`.`scheduledevents` 
CHANGE COLUMN `EventOccurrenceID` `EventOccurrenceID` BIGINT UNSIGNED NULL ;

ALTER TABLE `soft`.`scheduledevents` 
ADD CONSTRAINT `FK_EventOccurrenceScheduledEvents`
  FOREIGN KEY (`EventOccurrenceID`)
  REFERENCES `soft`.`eventoccurrences` (`ID`);