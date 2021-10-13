USE `Soft`;

DROP TABLE IF EXISTS `JobMappings`;

CREATE TABLE `JobMappings` (
	`ID`               BIGINT UNSIGNED    NOT NULL    AUTO_INCREMENT,
	`CustomJobID`	   VARCHAR(50)	      NOT NULL,    
	`HangfireJobID`	   VARCHAR(1000)      NOT NULL,
    
	CONSTRAINT    `PK_JobMappings`    PRIMARY KEY (`ID`)
);
