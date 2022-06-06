USE Soft;

LOCK TABLES eventoccurrences WRITE;

ALTER TABLE eventoccurrences

ADD UpdatedByAccountId BIGINT UNSIGNED    NOT NULL    DEFAULT 1;

UNLOCK TABLES;