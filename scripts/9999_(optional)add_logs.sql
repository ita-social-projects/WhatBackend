USE `Soft`;

DROP TABLE IF EXISTS `AccountsChanges`;

CREATE TABLE `AccountsChanges` (
    `ID`                        BIGINT UNSIGNED     NOT NULL         AUTO_INCREMENT,
    `AccountID`                 BIGINT UNSIGNED     NOT NULL,
    `OldRole`                   TINYINT UNSIGNED    DEFAULT NULL,
    `NewRole`                   TINYINT UNSIGNED    DEFAULT NULL,
    `OldFirstName`              VARCHAR(30)         DEFAULT NULL,
    `NewFirstName`              VARCHAR(30)         DEFAULT NULL,
    `OldLastName`               VARCHAR(30)         DEFAULT NULL,
    `NewLastName`               VARCHAR(30)         DEFAULT NULL,
    `OldEmail`                  VARCHAR(50)         DEFAULT NULL,
    `NewEmail`                  VARCHAR(50)         DEFAULT NULL,
    `OldPasswordHash`           VARCHAR(64)         DEFAULT NULL,
    `NewPasswordHash`           VARCHAR(64)         DEFAULT NULL,
    `OldSalt`                   VARCHAR(32)         DEFAULT NULL,
    `NewSalt`                   VARCHAR(32)         DEFAULT NULL,
    `OldIsActive`               BIT                 DEFAULT NULL,
    `NewIsActive`               BIT                 DEFAULT NULL,
    `OldForgotPasswordToken`    VARCHAR(36)         DEFAULT NULL,
    `NewForgotPasswordToken`    VARCHAR(36)         DEFAULT NULL,
    `OldForgotTokenGenDate`     DATETIME            DEFAULT NULL,
    `NewForgotTokenGenDate`     DATETIME            DEFAULT NULL,
    `OldAvatarID`               BIGINT UNSIGNED     DEFAULT NULL,
    `NewAvatarID`               BIGINT UNSIGNED     DEFAULT NULL,
    `QueriedBy`                 BIGINT UNSIGNED     NOT NULL,
    `DateTime`                  DATETIME     		DEFAULT NOW(),
    
    CONSTRAINT    `PK_AccountsChanges`         PRIMARY KEY (`ID`)
);

DELIMITER $$

CREATE TRIGGER `AfterAccountsInsert`
AFTER INSERT
ON `Accounts` FOR EACH ROW
BEGIN
	INSERT INTO `AccountsChanges`
    (`AccountID`, `NewRole`, `NewFirstName`, `NewLastName`, `NewEmail`, `NewPasswordHash`, `NewSalt`, `NewIsActive`, `NewForgotPasswordToken`, `NewForgotTokenGenDate`, `NewAvatarID`, `QueriedBy`)
    VALUES
    (NEW.`ID`, NEW.`Role`, NEW.`FirstName`, NEW.`LastName`, NEW.`Email`, NEW.`PasswordHash`, NEW.`Salt`, NEW.`IsActive`, NEW.`ForgotPasswordToken`, NEW.`ForgotTokenGenDate`, NEW.`AvatarID`, NEW.`LastEditorID`)
    ;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER `AfterAccountsUpdate`
AFTER UPDATE
ON `Accounts` FOR EACH ROW
BEGIN
	INSERT INTO `AccountsChanges`
    (`AccountID`, `OldRole`, `NewRole`, `OldFirstName`, `NewFirstName`, `OldLastName`, `NewLastName`, `OldEmail`, `NewEmail`, `OldPasswordHash`, `NewPasswordHash`, `OldSalt`, `NewSalt`, `OldIsActive`, `NewIsActive`, `OldForgotPasswordToken`, `NewForgotPasswordToken`,
    `OldForgotTokenGenDate`, `NewForgotTokenGenDate`, `OldAvatarID`, `NewAvatarID`, `QueriedBy`)
    VALUES
    (NEW.`ID`, OLD.`Role`, NEW.`Role`, OLD.`FirstName`, NEW.`FirstName`, OLD.`LastName`, NEW.`LastName`, OLD.`Email`, NEW.`Email`, OLD.`PasswordHash`, NEW.`PasswordHash`, OLD.`Salt`, NEW.`Salt`, OLD.`IsActive`, NEW.`IsActive`, OLD.`ForgotPasswordToken`, NEW.`ForgotPasswordToken`, 
    OLD.`ForgotTokenGenDate`, NEW.`ForgotTokenGenDate`, OLD.`AvatarID`, NEW.`AvatarID`, NEW.`LastEditorID`)
    ;
END$$

DELIMITER ;