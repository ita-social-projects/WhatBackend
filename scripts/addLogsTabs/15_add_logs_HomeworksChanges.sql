USE `Soft`;

DROP TABLE IF EXISTS `HomeworksChanges`;

CREATE TABLE `HomeworksChanges` (
    `ID`                        BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `HomeworkID`                BIGINT UNSIGNED     NOT NULL,
    `OldDueDate`                DATETIME           NOT NULL     COMMENT 'Use UTC time',
    `NewDueDate`                DATETIME           NOT NULL     COMMENT 'Use UTC time',
    `OldTaskText`               VARCHAR(8000)      NOT NULL,
    `NewTaskText`               VARCHAR(8000)      NOT NULL,
    `QueriedBy`                 BIGINT UNSIGNED     NOT NULL,
    `DateTime`                  DATETIME     		DEFAULT NOW(),

    CONSTRAINT    `PK_HomeworksChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterHomeworksInsert`
AFTER INSERT
ON `Homeworks` FOR EACH ROW
BEGIN
	INSERT INTO `HomeworksChanges`
    (`HomeworkID`, `NewDueDate`, `NewTaskText`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`DueDate`,  NEW.`TaskText`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterHomeworksUpdate`
AFTER UPDATE
ON `Homeworks` FOR EACH ROW
BEGIN
	INSERT INTO `HomeworksChanges`
    (`HomeworkID`, `OldDueDate`, `NewDueDate`,`OldTaskText`, `NewTaskText`, `QueriedBy`)
    VALUES
    (NEW.`ID`, OLD.`DueDate`, NEW.`DueDate`,  OLD.`TaskText`, NEW.`TaskText`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;