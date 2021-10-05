USE soft;

ALTER TABLE scheduledevents DROP FOREIGN KEY FK_EventOccurrenceScheduledEvents;
/*ALTER TABLE scheduledevents DROP FOREIGN KEY FK_LessonScheduledEvents;
ALTER TABLE scheduledevents DROP FOREIGN KEY FK_MentorScheduledEvents;
ALTER TABLE scheduledevents DROP FOREIGN KEY FK_StudentGroupScheduledEvents;
ALTER TABLE scheduledevents DROP FOREIGN KEY FK_ThemeScheduledEvents;
*/

DROP INDEX eventIndex
ON scheduledevents;

ALTER TABLE scheduledevents ADD CONSTRAINT    `FK_EventOccurrenceScheduledEvents`    FOREIGN KEY (`EventOccurrenceID`)    REFERENCES `EventOccurrences` (`ID`);
/*ALTER TABLE scheduledevents ADD CONSTRAINT    `FK_LessonScheduledEvents`             FOREIGN KEY (`LessonID`)             REFERENCES `Lessons` (`ID`);
ALTER TABLE scheduledevents ADD CONSTRAINT    `FK_MentorScheduledEvents`             FOREIGN KEY (`MentorID`)             REFERENCES `Mentors` (`ID`);
ALTER TABLE scheduledevents ADD CONSTRAINT    `FK_StudentGroupScheduledEvents`       FOREIGN KEY (`StudentGroupID`)       REFERENCES `StudentGroups` (`ID`);
ALTER TABLE scheduledevents ADD CONSTRAINT    `FK_ThemeScheduledEvents`              FOREIGN KEY (`ThemeID`)              REFERENCES `Themes` (`ID`);
*/