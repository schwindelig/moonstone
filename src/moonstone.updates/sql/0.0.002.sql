-- 0.0.2

CREATE TABLE [core].[users] (
	id UNIQUEIDENTIFIER NOT NULL,
	email NVARCHAR(256) NOT NULL,
	passwordHash NVARCHAR(MAX) NOT NULL,
	accessFailedCount INT NOT NULL,
	isLockoutEnabled BIT NOT NULL,
	lockoutEndDateUtc DATETIME2,

	PRIMARY KEY(id)
);

ALTER TABLE [core].[users] ADD
	CONSTRAINT [DEF_users_id]  DEFAULT (NEWSEQUENTIALID()) FOR [id],
	CONSTRAINT [DEF_accessFailedCount] DEFAULT(0) FOR [accessFailedCount],
	CONSTRAINT [DEF_isLockoutEnabled] DEFAULT(0) FOR [isLockoutEnabled],
	CONSTRAINT [UN_users_email] UNIQUE (email)