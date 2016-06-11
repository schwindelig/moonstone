-- 0.0.5

CREATE TABLE [core].[groups] (
	id UNIQUEIDENTIFIER NOT NULL,
	createDateUtc DATETIME2 NOT NULL,
	createUserId UNIQUEIDENTIFIER NOT NULL,
	name NVARCHAR(256) NOT NULL,
	description NVARCHAR(1024) NOT NULL,

	PRIMARY KEY(id)
);

ALTER TABLE [core].[groups] ADD
	CONSTRAINT [DEF_groups_id]  DEFAULT (NEWSEQUENTIALID()) FOR [id],
	CONSTRAINT [FK_users_id_creator] FOREIGN KEY ([createUserId]) REFERENCES [core].[users]([id]);