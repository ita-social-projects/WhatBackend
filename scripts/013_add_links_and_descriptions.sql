ALTER TABLE `soft`.`scheduledevents` 
ADD COLUMN `Description` VARCHAR(8000) NULL AFTER `EventFinish`,
ADD COLUMN `Link` VARCHAR(200) NULL AFTER `Description`;
