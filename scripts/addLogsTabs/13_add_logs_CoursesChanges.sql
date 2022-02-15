USE `Soft`;

DROP TABLE IF EXISTS `CoursesChanges`;

CREATE TABLE `CoursesChanges` (
    `ID`                        BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `CourseID`                 BIGINT UNSIGNED     NOT NULL,
    `OldName`                   TINYINT UNSIGNED    DEFAULT NULL,
    `NewName`                   TINYINT UNSIGNED    DEFAULT NULL,
    `OldIsActive`               BIT                 DEFAULT NULL,
    `NewIsActive`               BIT                 DEFAULT NULL,
    `QueriedBy`                 BIGINT UNSIGNED     NOT NULL,
    `DateTime`                  DATETIME     		DEFAULT NOW(),

    CONSTRAINT    `PK_CoursesChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterCoursesInsert`
AFTER INSERT
ON `Courses` FOR EACH ROW
BEGIN
	INSERT INTO `CoursesChanges`
    (`CourseID`, `NewName`, `NewIsActive`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`Name`,  NEW.`IsActive`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterCoursesUpdate`
AFTER UPDATE
ON `Courses` FOR EACH ROW
BEGIN
	INSERT INTO `CoursesChanges`
    (`CourseID`, `OldName`, `NewName`,`OldIsActive`, `NewIsActive`, `QueriedBy`)
    VALUES
    (NEW.`ID`, OLD.`Name`, NEW.`Name`,  OLD.`IsActive`, NEW.`IsActive`, NEW.`UpdatedByAccountId`)
    ;
END$$

DELIMITER ;