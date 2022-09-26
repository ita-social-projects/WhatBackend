USE soft;

ALTER TABLE scheduledevents DROP FOREIGN KEY FK_EventOccurrenceScheduledEvents;

DROP INDEX eventIndex
ON scheduledevents;

ALTER TABLE scheduledevents ADD CONSTRAINT    `FK_EventOccurrenceScheduledEvents`    FOREIGN KEY (`EventOccurrenceID`)    REFERENCES `EventOccurrences` (`ID`);
