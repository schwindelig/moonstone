-- 0.0.1

IF NOT EXISTS (SELECT loginname FROM [master].[dbo].[syslogins]
    where [name] = 'moonstone_ui')
Begin
-- create moonstone_ui_web login
CREATE LOGIN moonstone_ui WITH PASSWORD = 'moonstone_ui';
END;

-- create moonstone_ui_web user
USE ::db::;
CREATE USER moonstone_ui_web FOR LOGIN moonstone_ui;

-- create core schema
EXEC('CREATE SCHEMA [core];');

-- grant permissions to moonstone_ui_web to core
GRANT SELECT ON SCHEMA::[core] TO moonstone_ui_web;
GRANT UPDATE ON SCHEMA::[core] TO moonstone_ui_web;
GRANT INSERT ON SCHEMA::[core] TO moonstone_ui_web;