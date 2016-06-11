-- 0.0.4

-- add create date column
ALTER TABLE [core].[users]
	ADD createDateUtc DATETIME2 NOT NULL;