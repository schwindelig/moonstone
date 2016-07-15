-- 0.0.11

EXEC sp_RENAME '[core].[transactions].[creationDate]' , 'creationDateUtc', 'COLUMN';
EXEC sp_RENAME '[core].[transactions].[valueDate]' , 'valueDateUtc', 'COLUMN';

ALTER TABLE [core].[transactions] ALTER COLUMN [destinationBankAccountId] UNIQUEIDENTIFIER NULL;
ALTER TABLE [core].[transactions] ALTER COLUMN [description] NVARCHAR(1024) NULL;