USE `Soft`;

DROP TABLE IF EXISTS `MarksChanges`;

CREATE TABLE `MarksChanges` (
    `ID`                     BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `MarksID`                BIGINT UNSIGNED     NOT NULL,
    `OldValue`               TINYINT             DEFAULT NULL, 
    `NewValue`               TINYINT             DEFAULT NULL, 
    `OldComment`             VARCHAR(1024)       DEFAULT NULL,
    `NewComment`             VARCHAR(1024)       DEFAULT NULL,
    `OldEvaluationDate`      DATETIME            NOT NULL,
    `NewEvaluationDate`      DATETIME            NOT NULL,
    `OldType`                TINYINT UNSIGNED    NOT NULL         COMMENT 'Types:\n 0 - Homework, ...',
    `NewType`                TINYINT UNSIGNED    NOT NULL         COMMENT 'Types:\n 0 - Homework, ... ',
    `OldEvaluatedBy`         BIGINT UNSIGNED     NOT NULL,
	`NewEvaluatedBy`         BIGINT UNSIGNED     NOT NULL,
	`QueriedBy`              BIGINT UNSIGNED     NOT NULL,
    `DateTime`               DATETIME     		DEFAULT NOW(),
    CONSTRAINT    `PK_MarksChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterMarksInsert`
AFTER INSERT
ON `marks` FOR EACH ROW
BEGIN
	INSERT INTO `MarksChanges`
    (`MarksID`,`NewValue`,`NewComment`,`NewEvaluationDate`, `NewType`, `NewEvaluatedBy`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`Value`, NEW.`Comment`, NEW.`EvaluationDate`, NEW.`Type`, NEW.`EvaluatedBy`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterMarksUpdate`
AFTER UPDATE
ON `marks` FOR EACH ROW
BEGIN
	INSERT INTO `MarksChanges`
    (`MarksID`, `OldValue`, `NewValue`, `OldComment`, `NewComment`,`OldEvaluationDate`,`NewEvaluationDate`,`OldType`, `NewType`, `OldEvaluatedBy`, `NewEvaluatedBy`, `QueriedBy`)
    VALUES
    (NEW.`ID`, OLD.`Value`, NEW.`Value`, OLD.`Comment`, NEW.`Comment`,OLD.`EvaluationDate`,NEW.`EvaluationDate`, OLD.`Type`, NEW.`Type`, OLD.`EvaluatedBy`, NEW.`EvaluatedBy`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;