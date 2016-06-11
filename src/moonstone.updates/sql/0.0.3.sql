-- 0.0.3

-- add culture column
ALTER TABLE [core].[users]
	ADD culture NVARCHAR(16) NOT NULL;