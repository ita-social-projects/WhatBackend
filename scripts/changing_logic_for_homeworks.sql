USE `Soft`;

CREATE TABLE `Marks` (
    `ID`                BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
	`Value`             TINYINT             DEFAULT NULL,        
    `Comment`           VARCHAR(1024)       DEFAULT NULL,
    `EvaluationDate`    DATETIME            NOT NULL,
    `Type`              TINYINT UNSIGNED    NOT NULL         COMMENT 'Types:\n 0 - Homework,\n 1 - Visit',
    `EvaluatedBy`		BIGINT UNSIGNED    	NOT NULL,
	
    CONSTRAINT    `PK_Mark`             PRIMARY KEY (`ID`),
    CONSTRAINT    `FK_AccountOfMark`    FOREIGN KEY (`EvaluatedBy`)    REFERENCES `Accounts` (`ID`)
);

ALTER TABLE `HomeworksFromStudents` 
    ADD    `MarkID` 		   BIGINT UNSIGNED    DEFAULT NULL,
    ADD    `PublishingDate`    DATETIME           NOT NUll,
    ADD    `IsSent`			   BOOLEAN 	          NOT NULL,
    
    ADD CONSTRAINT    `FK_MarkOfHomeworkFromStudent`    FOREIGN KEY (`MarkID`)    REFERENCES `Marks` (`ID`);
  
ALTER TABLE `Homeworks`
    ADD    `PublishingDate`    DATETIME           NOT NULL,
    ADD    `CreatedBy`   	   BIGINT UNSIGNED    NOT NULL,
    
    ADD CONSTRAINT    `FK_AccountOfHomework`    FOREIGN KEY (`CreatedBy`)    REFERENCES `Accounts` (`ID`);
    