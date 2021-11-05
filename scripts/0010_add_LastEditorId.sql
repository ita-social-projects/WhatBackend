USE `Soft`;

LOCK TABLES `Visits` WRITE;

ALTER TABLE `Visits` 
	ADD		`LastEditorID`		BIGINT UNSIGNED		NOT NULL		DEFAULT 1;

UNLOCK TABLES;

LOCK TABLES `Accounts` WRITE;

ALTER TABLE `Accounts` 
	ADD		`LastEditorID`		BIGINT UNSIGNED		NOT NULL		DEFAULT 1;

UNLOCK TABLES;