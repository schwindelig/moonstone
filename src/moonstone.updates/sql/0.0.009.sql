-- 0.0.9

CREATE TABLE [core].[bankAccounts] (
	[id] UNIQUEIDENTIFIER NOT NULL,
	[groupId] UNIQUEIDENTIFIER NOT NULL,
	[name] nvarchar(256) NOT NULL,
	[description] nvarchar(2048) NOT NULL,
	[createUserId] UNIQUEIDENTIFIER NOT NULL,

	PRIMARY KEY(id)
);

ALTER TABLE [core].[bankAccounts] ADD
	CONSTRAINT [DEF_bankAccounts_id]  DEFAULT (NEWSEQUENTIALID()) FOR [id],
	CONSTRAINT [FK_bankAccounts_groups_id] FOREIGN KEY ([groupId]) REFERENCES [core].[groups]([id]),
	CONSTRAINT [FK_bankAccounts_users_id_creator] FOREIGN KEY ([createUserId]) REFERENCES [core].[users]([id]);