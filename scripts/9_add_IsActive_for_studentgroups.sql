USE `Soft`;

LOCK TABLES `StudentGroups` WRITE;

ALTER TABLE `StudentGroups` 
	ADD		`IsActive`		BIT(1)		NOT NULL		DEFAULT 1;

UNLOCK TABLES;
