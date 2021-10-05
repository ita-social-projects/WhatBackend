use soft;

ALTER TABLE visits DROP FOREIGN KEY FK_LessonVisits;
ALTER TABLE visits DROP FOREIGN KEY FK_StudentVisits;

DROP INDEX visitsIndex ON visits;

ALTER TABLE visits ADD CONSTRAINT    `FK_LessonVisits`     FOREIGN KEY (`LessonID`)            REFERENCES `Lessons` (`ID`);
ALTER TABLE visits ADD CONSTRAINT    `FK_StudentVisits`   	FOREIGN KEY (`StudentID`)           REFERENCES `Students` (`ID`);