USE soft;

/*ALTER TABLE homeworksfromstudents DROP FOREIGN KEY FK_StudentOfHomeworks;*/

DROP INDEX marksIndex ON homeworksfromstudents;

/*ALTER TABLE homeworksfromstudents ADD CONSTRAINT    `FK_StudentOfHomeworks`    FOREIGN KEY (`StudentID`)	REFERENCES `Students` (`ID`);
*/