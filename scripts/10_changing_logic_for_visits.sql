USE `Soft`;

ALTER TABLE `Visits`
    ADD    `MarkID` 	       BIGINT UNSIGNED,
    ADD CONSTRAINT    `FK_MarkID`    FOREIGN KEY (`MarkID`)    REFERENCES `Marks` (`ID`);

DROP PROCEDURE IF EXISTS cursorVisit;
DELIMITER //
CREATE PROCEDURE cursorVisit()
BEGIN

	DECLARE visitID BIGINT;
	DECLARE studentMark TINYINT;
	DECLARE commentVisit VARCHAR(1024);
	DECLARE evaluationDate DATETIME;
	DECLARE evaluatedBy BIGINT;
	DECLARE markID BIGINT;
	DECLARE done INT DEFAULT 0;
	DECLARE cursor1 CURSOR FOR SELECT V.`ID`, V.`StudentMark`, V.`Comment`, L.`LessonDate`, M.`AccountID` 
		FROM Visits V
		INNER JOIN Lessons L 
		ON L.`ID` = V.`LessonID`
		INNER JOIN `Mentors` M
		WHERE L.`MentorID` = M.`ID` AND `MarkID` IS NULL;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

	OPEN cursor1;
 
	REPEAT 
		FETCH cursor1 INTO visitID, studentMark, commentVisit, evaluationDate, evaluatedBy;
		IF NOT done THEN 
			INSERT INTO `Marks` (`Value`, `Comment`, `Type`, `EvaluationDate`, `EvaluatedBy`)
			VALUES (studentMark, commentVisit, 1, evaluationDate, evaluatedBy);
			SET markID = LAST_INSERT_ID();
			UPDATE `Visits` AS V
			SET V.`MarkID` = markID WHERE V.`ID` = visitID;
		END IF;
		
	UNTIL done END REPEAT;
	CLOSE cursor1; 
 
END;
CALL cursorVisit();

ALTER TABLE `Visits`
	DROP COLUMN    `StudentMark`,
    DROP COLUMN    `Comment`;

