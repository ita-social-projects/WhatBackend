USE `Soft`;

LOCK TABLES `Accounts` WRITE;

ALTER TABLE `Accounts` 
ADD `TelegramID` VARCHAR(8000) NULL;

ALTER TABLE `Accounts` 
ADD `TelegramToken` VARCHAR(36) NULL COMMENT 'GUID length is 36 characters';

ALTER TABLE `Accounts` 
ADD `TelegramTokenGenDate` DATETIME NULL COMMENT 'Use UTC time';

UNLOCK TABLES;