-- 0.0.8

CREATE TABLE [core].[categories] (
	[id] UNIQUEIDENTIFIER NOT NULL,
	[groupId] UNIQUEIDENTIFIER NOT NULL,
	[createUserId] UNIQUEIDENTIFIER NOT NULL,
	[name] nvarchar(256) NOT NULL,

	PRIMARY KEY(id)
);

ALTER TABLE [core].[categories] ADD
	CONSTRAINT [DEF_categories_id]  DEFAULT (NEWSEQUENTIALID()) FOR [id],
	CONSTRAINT [FK_categories_groups_id] FOREIGN KEY ([groupId]) REFERENCES [core].[groups]([id]),
	CONSTRAINT [FK_categories_users_id_creator] FOREIGN KEY ([createUserId]) REFERENCES [core].[users]([id]);