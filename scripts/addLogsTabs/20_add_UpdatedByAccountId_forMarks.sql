USE Soft;

LOCK TABLES marks WRITE;

ALTER TABLE marks

ADD UpdatedByAccountId BIGINT UNSIGNED    NOT NULL    DEFAULT 1;

UNLOCK TABLES;