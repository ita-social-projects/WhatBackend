USE `Soft`;

LOCK TABLES `Visits` WRITE;

ALTER TABLE `Visits` 
	ADD		`LastEditorID`		BIGINT UNSIGNED		NOT NULL		DEFAULT 1;

UNLOCK TABLES;

DROP TABLE IF EXISTS `VisitsChanges`;

CREATE TABLE `VisitsChanges` (
    `ID`                BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `VisitID`           BIGINT UNSIGNED     NOT NULL,
    `OldStudentID`      BIGINT UNSIGNED     DEFAULT NULL,
    `NewStudentID`      BIGINT UNSIGNED     DEFAULT NULL,
    `OldLessonID`       BIGINT UNSIGNED     DEFAULT NULL,
    `NewLessonID`       BIGINT UNSIGNED     DEFAULT NULL,
    `OldStudentMark`    TINYINT UNSIGNED    DEFAULT NULL,
    `NewStudentMark`    TINYINT UNSIGNED    DEFAULT NULL,
    `OldPresence`       BIT                 DEFAULT NULL,
    `NewPresence`       BIT                 DEFAULT NULL,
    `OldComment`        VARCHAR(1024)       DEFAULT NULL,
    `NewComment`        VARCHAR(1024)       DEFAULT NULL,
    `QueriedBy`         BIGINT UNSIGNED     NOT NULL,

    CONSTRAINT    `PK_VisitChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterVisitsInsert`
AFTER INSERT
ON `Visits` FOR EACH ROW
BEGIN
	INSERT INTO `VisitsChanges`
    (`VisitID`, `NewStudentID`, `NewLessonId`, `NewStudentMark`, `NewPresence`, `NewComment`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`StudentID`, NEW.`LessonID`, NEW.`StudentMark`, NEW.`Presence`, NEW.`Comment`, NEW.`LastEditorID`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterVisitsDelete`
AFTER DELETE
ON `Visits` FOR EACH ROW
BEGIN
	INSERT INTO `VisitsChanges`
    (`VisitID`, `OldStudentID`, `OldLessonId`, `OldStudentMark`, `OldPresence`, `OldComment`, `QueriedBy`)
    VALUES
    (OLD.`ID`, OLD.`StudentID`, OLD.`LessonID`, OLD.`StudentMark`, OLD.`Presence`, OLD.`Comment`, OLD.`LastEditorID`)
    ;
END$$

DELIMITER ;