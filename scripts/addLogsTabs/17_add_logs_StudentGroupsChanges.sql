USE `Soft`;

DROP TABLE IF EXISTS `StudentGroupsChanges`;

CREATE TABLE `StudentGroupsChanges` (
    `ID`                        BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `StudentGroupID`            BIGINT UNSIGNED     NOT NULL,
    `OldName`                   VARCHAR(100)       NOT NULL,
    `NewName`                   VARCHAR(100)       NOT NULL,
    `OldStartDate`              DATE               NOT NULL,
    `NewStartDate`              DATE               NOT NULL,
    `OldFinishDate`             DATE               NOT NULL,
    `NewFinishDate`             DATE               NOT NULL,
    `OldIsActive`               BIT                 DEFAULT NULL,
    `NewIsActive`               BIT                 DEFAULT NULL,
	`QueriedBy`                 BIGINT UNSIGNED     NOT NULL,
    `DateTime`                  DATETIME     		DEFAULT NOW(),
    CONSTRAINT    `PK_StudentGroupsChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterStudentGroupsInsert`
AFTER INSERT
ON `studentgroups` FOR EACH ROW
BEGIN
	INSERT INTO `StudentGroupsChanges`
    (`StudentGroupID`,`NewName`,`NewStartDate`,`NewFinishDate`, `NewIsActive`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`Name`,  NEW.`StartDate`, NEW.`FinishDate`, NEW.`IsActive`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterStudentGroupsUpdate`
AFTER UPDATE
ON `studentgroups` FOR EACH ROW
BEGIN
	INSERT INTO `StudentGroupsChanges`
    (`StudentGroupID`, `OldName`, `NewName`, `OldStartDate`, `NewStartDate`,`OldFinishDate`,`NewFinishDate`,`OldIsActive`, `NewIsActive`, `QueriedBy`)
    VALUES
    (NEW.`ID`, OLD.`Name`, NEW.`Name`, OLD.`StartDate`, NEW.`StartDate`,OLD.`FinishDate`,NEW.`FinishDate`, OLD.`IsActive`, NEW.`IsActive`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;