USE `Soft`;

DROP TABLE IF EXISTS `EventOccurrencesChanges`;

CREATE TABLE `EventOccurrencesChanges` (
    `ID`                        BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `EventOccurrencesID`          BIGINT UNSIGNED     NOT NULL,
    `OldStudentGroupID`           BIGINT UNSIGNED     NOT NULL,
    `NewStudentGroupID`           BIGINT UNSIGNED     NOT NULL,
    `OldSEventStart`            DATETIME            NOT NULL        COMMENT 'Use UTC time',
    `NewEventStart`             DATETIME            NOT NULL        COMMENT 'Use UTC time',
    `OldEventFinish`            DATETIME            NOT NULL        COMMENT 'Use UTC time',
    `NewEventFinish`            DATETIME            NOT NULL        COMMENT 'Use UTC time',
    `OldPattern`                TINYINT UNSIGNED    DEFAULT NULL    COMMENT 'Patterns:\n0 - Daily,\n1 - Weekly,\n2 - AbsoluteMonthly,\n3 - RelativeMonthly',
    `NewPattern`                TINYINT UNSIGNED    DEFAULT NULL    COMMENT 'Patterns:\n0 - Daily,\n1 - Weekly,\n2 - AbsoluteMonthly,\n3 - RelativeMonthly',
	`QueriedBy`                 BIGINT UNSIGNED     NOT NULL,
    `DateTime`                  DATETIME     		DEFAULT NOW(),
    CONSTRAINT    `PK_EventOccurrencesChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterEventOccurrencesInsert`
AFTER INSERT
ON `eventoccurrences` FOR EACH ROW
BEGIN
	INSERT INTO `EventOccurrencesChanges`
    (`EventOccurrencesID`,`NewStudentGroupID`,`NewEventStart`,`NewEventFinish`, `NewPattern`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`StudentGroupID`,  NEW.`EventStart`, NEW.`EventFinish`, NEW.`Pattern`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterEventOccurrencesUpdate`
AFTER UPDATE
ON `eventoccurrences` FOR EACH ROW
BEGIN
	INSERT INTO `EventOccurrencesChanges`
    (`EventOccurrencesID`, `OldStudentGroupID`, `NewStudentGroupID`, `OldEventStart`, `NewEventStart`,`OldEventFinish`,`NewEventFinish`,`OldPattern`, `NewPattern`, `QueriedBy`)
    VALUES
    (NEW.`ID`, OLD.`StudentGroupID`, NEW.`StudentGroupID`, OLD.`EventStart`, NEW.`EventStart`,OLD.`EventFinish`,NEW.`EventFinish`, OLD.`Pattern`, NEW.`Pattern`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;