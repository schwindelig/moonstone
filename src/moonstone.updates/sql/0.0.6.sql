-- 0.0.6

CREATE TABLE [core].[groupUsers] (
	groupId UNIQUEIDENTIFIER NOT NULL,
	userId UNIQUEIDENTIFIER NOT NULL,

	PRIMARY KEY(groupId, userId)
);

ALTER TABLE [core].[groupUsers] ADD
	CONSTRAINT [FK_groups_id] FOREIGN KEY ([groupId]) REFERENCES [core].[groups]([id]),
	CONSTRAINT [FK_users_id] FOREIGN KEY ([userId]) REFERENCES [core].[users]([id]);