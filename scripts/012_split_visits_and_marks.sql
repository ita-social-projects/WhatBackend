USE `Soft`;

/*1. Add foreign key in visits to marks*/
ALTER TABLE `Visits` 
    ADD    `MarkID` 	       BIGINT UNSIGNED    DEFAULT NULL,    
    ADD CONSTRAINT    `FK_MarkOfVisits`    FOREIGN KEY (`MarkID`)    REFERENCES `Marks` (`ID`);

/*2. go through visits, add marks and comments to table marks, add id of created row to visits*/
DELIMITER $$

CREATE PROCEDURE UpdateData()
BEGIN
DECLARE finished INTEGER DEFAULT 0;
DECLARE CommentVar VARCHAR(1024);
DECLARE StudentMarkVar TINYINT;
DECLARE IdVar BIGINT;
DECLARE DateTimeNow CHAR(50);

DECLARE CursorVisits CURSOR FOR SELECT Visits.Id, Visits.StudentMark, Visits.Comment FROM Visits;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET finished = 1;
SELECT CAST(NOW() AS CHAR(50)) INTO @DateTimeNow;

OPEN CursorVisits;

labelGetVisits: LOOP
IF finished = 1 THEN
	LEAVE labelGetVisits;
    END IF;

 /*Get cursor data into variables*/   
FETCH CursorVisits INTO IdVar, StudentMarkVar, CommentVar; 

/*Insert data from visits to marks*/
INSERT INTO Marks (Value, Comment, EvaluationDate, Type, EvaluatedBy) 
	VALUES (StudentMarkVar, CommentVar, @DateTimeNow, 0, (SELECT AccountID FROM Mentors ORDER BY RAND() LIMIT 1/*Get random AccountID*/));

/*Create link of last added marks row to visits*/
UPDATE Visits SET Visits.MarkID = LAST_INSERT_ID()/*Get Id of last added row*/ WHERE ID = IdVar;

END LOOP labelGetVisits;

CLOSE CursorVisits;

END$$
DELIMITER ;

CALL UpdateData();
DROP PROCEDURE UpdateData;

/*3. drop columns mark and comment from table visits*/
ALTER TABLE Visits
DROP COLUMN StudentMark,
DROP COLUMN Comment;
