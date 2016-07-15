-- 0.0.10

CREATE TABLE [core].[transactions] (
	[id] UNIQUEIDENTIFIER NOT NULL,
	[createUserId] UNIQUEIDENTIFIER NOT NULL,
	[groupId] UNIQUEIDENTIFIER NOT NULL,
	[categoryId] UNIQUEIDENTIFIER NOT NULL,
	[sourceBankAccountId] UNIQUEIDENTIFIER NOT NULL,
	[destinationBankAccountId] UNIQUEIDENTIFIER NOT NULL,
	[creationDate] DATETIME2 NOT NULL,
	[valueDate] DATETIME2 NOT NULL,
	[isBooked] BIT NOT NULL,
	[amount] MONEY NOT NULL,
	[currency] NVARCHAR(16) NOT NULL,
	[title] NVARCHAR(128) NOT NULL,
	[description] NVARCHAR(1028) NOT NULL

	PRIMARY KEY(id)
);

ALTER TABLE [core].[transactions] ADD
	CONSTRAINT [DEF_transactions_id]  DEFAULT (NEWSEQUENTIALID()) FOR [id],
	CONSTRAINT [FK_transactions_createUserId] FOREIGN KEY ([createUserId]) REFERENCES [core].[users]([id]),
	CONSTRAINT [FK_transactions_groupId] FOREIGN KEY ([groupId]) REFERENCES [core].[groups]([id]),
	CONSTRAINT [FK_transactions_categoryId] FOREIGN KEY ([categoryId]) REFERENCES [core].[categories]([id]),
	CONSTRAINT [FK_transactions_sourceBankAccountId] FOREIGN KEY ([sourceBankAccountId]) REFERENCES [core].[bankAccounts]([id]),
	CONSTRAINT [FK_transactions_destinationBankAccountId] FOREIGN KEY ([destinationBankAccountId]) REFERENCES [core].[bankAccounts]([id]);