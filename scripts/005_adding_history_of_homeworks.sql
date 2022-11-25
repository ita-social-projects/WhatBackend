USE `Soft`;

DROP TABLE IF EXISTS `HomeworksFromStudentsHistory`;

CREATE TABLE `HomeworksFromStudentsHistory` (
    `ID`                       BIGINT UNSIGNED    NOT NULL     AUTO_INCREMENT,
    `HomeworkText`             VARCHAR(8000),
    `HomeworkFromStudentID`    BIGINT UNSIGNED    NOT NULL,
    `MarkID`         		   BIGINT UNSIGNED    NOT NULL,     
    `PublishingDate`           DATETIME           NOT NULL,

    CONSTRAINT    `PK_HomeworksFromStudentsHistory`    PRIMARY KEY (`ID`),
    CONSTRAINT    `FK_HomeworkStudentOfHistory`    	   FOREIGN KEY (`HomeworkFromStudentID`)    REFERENCES `HomeworksFromStudents` (`ID`),
    CONSTRAINT    `FK_MarkOfHitory`    				   FOREIGN KEY (`MarkID`)    				REFERENCES `Marks` (`ID`)
);


DROP TABLE IF EXISTS `AttachmentsOfHomeworksFromStudentsHistory`;

CREATE TABLE IF NOT EXISTS `AttachmentsOfHomeworksFromStudentsHistory` (
    `ID`                       		  BIGINT UNSIGNED    NOT NULL     AUTO_INCREMENT,
    `AttachmentID`             		  BIGINT UNSIGNED    NOT NULL,
    `HomeworkFromStudentHistoryID`    BIGINT UNSIGNED    NOT NULL,

    CONSTRAINT    `PK_AttachmentOfHomeworkFromStudentHistory`      PRIMARY KEY (`ID`),
    CONSTRAINT    `FK_AttachmentOfHomeworksFromStudentsHistory`    FOREIGN KEY (`AttachmentID`)                    REFERENCES `Attachments` (`ID`),
    CONSTRAINT    `FK_HomeworkFromStudentHistoryOfAttachments`     FOREIGN KEY (`HomeworkFromStudentHistoryID`)    REFERENCES `HomeworksFromStudentsHistory` (`ID`),
    CONSTRAINT    `UQ_HomeworkFromStudentAndAttachment`            UNIQUE (`AttachmentID`, `HomeworkFromStudentHistoryID`),

    INDEX    `IX_HomeworkFromStudentHistory`    (`HomeworkFromStudentHistoryID` ASC),
    INDEX    `IX_AttachmentHistory`            	(`AttachmentID` ASC)
);