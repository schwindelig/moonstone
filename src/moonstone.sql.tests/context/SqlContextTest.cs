using FluentAssertions;
using moonstone.core.exceptions;
using moonstone.sql.context;
using NUnit.Framework;
using System;
using System.Linq;

namespace moonstone.sql.test.context
{
    [TestFixture]
    public class SqlContextTest
    {
        private const string DATABASE_NAME = "moonstone_tests";
        private const string SERVER_ADDRESS = ".";

        public static SqlContext GetInitializedContext()
        {
            var validContext = GetValidContext();
            validContext.Init();

            return validContext;
        }

        public static SqlContext GetValidContext()
        {
            return new SqlContext(DATABASE_NAME, SERVER_ADDRESS);
        }

        [Test]
        public void Can_Add_Installed_Version_With_Transaction()
        {
            var initializedContext = GetInitializedContext();
            var versionToInstall = new SqlVersion(1, 2, 3);
            var script = new SqlScript(
                "add_version",
                "select 42;",
                versionToInstall,
                useSpecifiedDatabase: true,
                useTransaction: true);

            initializedContext.ExecuteScript(script);

            var installedVersion = initializedContext.GetInstalledVersion().GetVersion();

            installedVersion.ShouldBeEquivalentTo(versionToInstall);
        }

        [Test]
        public void Can_Add_Installed_Version_Without_Transaction()
        {
            var initializedContext = GetInitializedContext();
            var versionToInstall = new SqlVersion(1, 2, 3);
            var script = new SqlScript(
                "add_version",
                "select 42;",
                versionToInstall,
                useSpecifiedDatabase: true,
                useTransaction: false);

            initializedContext.ExecuteScript(script);

            var installedVersion = initializedContext.GetInstalledVersion().GetVersion();

            installedVersion.ShouldBeEquivalentTo(versionToInstall);
        }

        [Test]
        public void Can_Add_Version()
        {
            var initializedContext = GetInitializedContext();
            var toAdd = new SqlInstalledVersion(1, 2, 3, DateTime.UtcNow);

            initializedContext.AddInstalledVersion(toAdd);

            var installed = initializedContext.GetInstalledVersion();

            installed.ShouldBeEquivalentTo(toAdd, options =>
                options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(toAdd.InstallDateUtc)).WhenTypeIs<DateTime>());
        }

        [Test]
        public void Can_Build_Command_Master()
        {
            var validContext = GetValidContext();
            string command = "SELECT 42";
            string expected = $"USE master; {command}";

            string result = validContext.BuildCommand(command, false);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Can_Build_Command_Specific()
        {
            var validContext = GetValidContext();
            string command = "SELECT 42";
            var expected = $"USE {DATABASE_NAME}; {command}";

            string result = validContext.BuildCommand(command, true);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Can_Create_DB()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }

            try
            {
                validContext.CreateDatabase();
                Assert.IsTrue(validContext.Exists());
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Can_Create_Login()
        {
            var validContext = GetValidContext();

            validContext.CreateLogin("user_1", "p@ssw0rd");
            validContext.RemoveLogin("user_1");
        }

        [Test]
        public void Can_Create_Version_Table()
        {
            var validContext = GetValidContext();

            validContext.CreateVersionTable();
            bool exists = validContext.VersionTableExists();

            Assert.IsTrue(exists);
        }

        [Test]
        public void Can_Drop_DB()
        {
            var validContext = GetValidContext();
            try
            {
                validContext.DropDatabase();
                Assert.IsFalse(validContext.Exists());
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Can_Drop_Table_On_Specified()
        {
            var initializedContext = GetInitializedContext();

            bool existsBeforeDrop;
            bool existsAfterDrop;

            existsBeforeDrop = initializedContext.VersionTableExists();
            initializedContext.DropTable(initializedContext.GetVersionTableName(), useSpecifiedDatabase: true);
            existsAfterDrop = initializedContext.VersionTableExists();

            Assert.IsTrue(existsBeforeDrop);
            Assert.IsFalse(existsAfterDrop);
        }

        [Test]
        public void Can_Drop_Table_On_Unspecified()
        {
            string tableName = "hurr";
            var initializedContext = GetInitializedContext();

            bool existsBeforeDrop;
            bool existsAfterDrop;

            var createTableScript = new SqlScript("hurr_create",
                $@"CREATE TABLE {tableName} (
                    id INT NOT NULL,
                    [name] NVARCHAR(128) NOT NULL,
                    PRIMARY KEY(id)
                );",
                new SqlVersion(1, 1, 1),
                useSpecifiedDatabase: false,
                useTransaction: false);

            if (!initializedContext.TableExists(tableName, useSpecifiedDatabase: false))
            {
                initializedContext.ExecuteScript(createTableScript);
            }

            existsBeforeDrop = initializedContext.TableExists(tableName, useSpecifiedDatabase: false);
            initializedContext.DropTable(tableName, useSpecifiedDatabase: false);
            existsAfterDrop = initializedContext.TableExists(tableName, useSpecifiedDatabase: false);

            Assert.IsTrue(existsBeforeDrop);
            Assert.IsFalse(existsAfterDrop);
        }

        [Test]
        public void Can_Execute_On_Specified()
        {
            var initializedContext = SqlContextTest.GetInitializedContext();

            bool tableExistsBeforeExecution;
            bool tableExistsAfterExecution;

            string tableName = "TEST";

            var script = new SqlScript("ttt",
                $@"CREATE TABLE {tableName} (
                        id int not null,
                        [name] nvarchar(128) not null
                        PRIMARY KEY(id)
                    );

                    INSERT INTO {tableName} (id, [name]) values (1, 'test');",
                new SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            tableExistsBeforeExecution = initializedContext.TableExists(tableName, useSpecifiedDatabase: true);
            initializedContext.ExecuteScript(script);
            tableExistsAfterExecution = initializedContext.TableExists(tableName, useSpecifiedDatabase: true);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Execute_On_Unspecified()
        {
            var initializedContext = GetInitializedContext();

            bool tableExistsBeforeExecution;
            bool tableExistsAfterExecution;

            string tableName = "test";

            var script = new SqlScript("ttt",
                $@"CREATE TABLE {tableName} (
                        id int not null,
                        [name] nvarchar(128) not null
                        PRIMARY KEY(id)
                    );

                INSERT INTO {tableName} (id, [name]) values (1, 'test');",
                new SqlVersion(1, 0, 0),
                useSpecifiedDatabase: false,
                useTransaction: false);

            if (initializedContext.TableExists(tableName, useSpecifiedDatabase: false))
            {
                initializedContext.DropTable(tableName, useSpecifiedDatabase: false);
            }

            tableExistsBeforeExecution = initializedContext.TableExists(tableName, useSpecifiedDatabase: false);
            initializedContext.ExecuteScript(script);
            tableExistsAfterExecution = initializedContext.TableExists(tableName, useSpecifiedDatabase: false);
            initializedContext.DropTable(tableName, useSpecifiedDatabase: false);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Execute_With_Transaction()
        {
            string tableName = "animals";
            bool tableExistsBeforeExecution = false;
            bool tableExistsAfterExecution = false;
            var initializedContext = GetInitializedContext();
            var script = new SqlScript(
                "animals",
                $@"CREATE TABLE {tableName}(
                    id int not null,
                    [name] nvarchar(128) not null,
                    PRIMARY KEY (id)
                );

                INSERT INTO {tableName} (id, [name]) VALUES(1, 'dog');",
                version: new SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: true);

            tableExistsBeforeExecution = initializedContext.TableExists(tableName, true);
            initializedContext.ExecuteScript(script);
            tableExistsAfterExecution = initializedContext.TableExists(tableName, true);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Execute_Without_Transaction()
        {
            string tableName = "animals";
            bool tableExistsBeforeExecution = false;
            bool tableExistsAfterExecution = false;
            var initializedContext = GetInitializedContext();
            var script = new SqlScript(
                "animals",
                $@"CREATE TABLE {tableName}(
                    id int not null,
                    [name] nvarchar(128) not null,
                    PRIMARY KEY (id)
                );

                INSERT INTO {tableName} (id, [name]) VALUES(1, 'dog');",
                version: new SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            tableExistsBeforeExecution = initializedContext.TableExists(tableName, true);
            initializedContext.ExecuteScript(script);
            tableExistsAfterExecution = initializedContext.TableExists(tableName, true);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Find_DB()
        {
            var validContext = GetValidContext();

            Assert.IsTrue(validContext.Exists());
        }

        [Test]
        public void Can_Find_Specified_Table()
        {
            var initializedContext = GetInitializedContext();

            bool exists = initializedContext.TableExists(initializedContext.GetVersionTableName(), true);

            Assert.IsTrue(exists);
        }

        [Test]
        public void Can_Find_Unspecified_Table()
        {
            var initializedContext = GetInitializedContext();

            bool exists = initializedContext.TableExists("spt_monitor", false);

            Assert.IsTrue(exists);
        }

        [Test]
        public void Can_Get_Latest_Version()
        {
            var initializedContext = GetInitializedContext();
            var ver1 = new SqlInstalledVersion(1, 0, 0, DateTime.Now);
            var ver2 = new SqlInstalledVersion(1, 2, 3, DateTime.Now);
            var ver3 = new SqlInstalledVersion(2, 0, 0, DateTime.Now);

            initializedContext.AddInstalledVersion(ver1);
            initializedContext.AddInstalledVersion(ver2);
            initializedContext.AddInstalledVersion(ver3);

            var installed = initializedContext.GetInstalledVersion();

            installed.ShouldBeEquivalentTo(ver3, options =>
                options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(ver3.InstallDateUtc)).WhenTypeIs<DateTime>());
        }

        [Test]
        public void Can_Not_Add_Lower_Version()
        {
            var initializedContext = GetInitializedContext();
            var ver1 = new SqlInstalledVersion(2, 0, 0, DateTime.UtcNow);
            var ver2 = new SqlInstalledVersion(1, 9, 9, DateTime.UtcNow);
            initializedContext.AddInstalledVersion(ver1);

            Assert.Throws<LowerOrEqualVersionException>(() => initializedContext.AddInstalledVersion(ver2));
        }

        [Test]
        public void Can_Not_Find_Specified_Table()
        {
            var initializedContext = GetInitializedContext();

            bool exists = initializedContext.TableExists("somenonexistenttable", true);

            Assert.IsFalse(exists);
        }

        [Test]
        public void Can_Not_Find_Unspecified_Table()
        {
            var initializedContext = GetInitializedContext();

            bool exists = initializedContext.TableExists("something", false);

            Assert.IsFalse(exists);
        }

        [Test]
        public void Can_Open_Valid_Connection()
        {
            var validContext = GetValidContext();

            var connection = validContext.OpenConnection();

            Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
        }

        [Test]
        public void Can_Read_Server_Version()
        {
            var validContext = GetValidContext();

            string version = validContext.ServerVersion();

            Assert.That(version.Contains("Microsoft SQL Server"));
        }

        [Test]
        public void Can_Remove_Login()
        {
            var validContext = GetValidContext();

            validContext.CreateLogin("toBeRemoved", "removeMe!!!11");
            validContext.RemoveLogin("toBeRemoved");
        }

        [Test]
        public void Can_Return_Version_Table_Name()
        {
            var initializedContext = GetInitializedContext();
            var versionTableName = initializedContext.GetVersionTableName();

            Assert.AreEqual("db_version", versionTableName);
        }

        [Test]
        public void Can_Roll_Back_Command()
        {
            var initializedContext = GetInitializedContext();

            try
            {
                var command = $@"
                CREATE TABLE ttt(id int not null, PRIMARY KEY(id));
                INSERT INTO ttt VALUES ('one');";

                initializedContext.RunCommand<dynamic>(command: command, param: null, mode: CommandMode.Write);
            }
            catch
            {
                Assert.That(!initializedContext.TableExists("ttt", useSpecifiedDatabase: true));
                return;
            }

            Assert.Fail("should not get here");
        }

        [Test]
        public void Can_Run_Multiple_Queries()
        {
            var validContext = GetValidContext();

            try
            {
                validContext.Exists();
                validContext.ServerVersion();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Can_Run_Read_Command()
        {
            var initializedContext = GetInitializedContext();
            string command = $"SELECT * FROM {initializedContext.GetVersionTableName()};";

            var result = initializedContext.RunCommand<SqlInstalledVersion>(command: command, param: null, mode: CommandMode.Read);

            Assert.That(result.Count() >= 1);
        }

        [Test]
        public void Can_Run_Read_Command_With_Param()
        {
            var initializedContext = GetInitializedContext();

            initializedContext.AddInstalledVersion(new SqlInstalledVersion(1, 2, 3, DateTime.UtcNow));
            initializedContext.AddInstalledVersion(new SqlInstalledVersion(2, 2, 3, DateTime.UtcNow));
            initializedContext.AddInstalledVersion(new SqlInstalledVersion(3, 2, 3, DateTime.UtcNow));
            initializedContext.AddInstalledVersion(new SqlInstalledVersion(4, 2, 3, DateTime.UtcNow));

            var command = $"SELECT * FROM {initializedContext.GetVersionTableName()} WHERE major > @major";
            var param = new { major = 2 };

            var result = initializedContext.RunCommand<SqlInstalledVersion>(command: command, param: param, mode: CommandMode.Read);

            Assert.That(result.Count() == 2);
        }

        [Test]
        public void Can_Run_Write_Command()
        {
            var validContext = GetValidContext();
            string command =
                $@"CREATE TABLE animals(
                    id int not null,
                    [name] nvarchar(128) not null,
                    PRIMARY KEY(id)
                );

                INSERT INTO animals (id, [name])
                VALUES
                    (1, 'snake'),
                    (2, 'zebra'),
                    (3, 'cat'),
                    (4, 'nessie')";

            validContext.RunCommand<dynamic>(command, param: null, mode: CommandMode.Write);

            Assert.That(validContext.TableExists("animals", useSpecifiedDatabase: true));
        }

        [Test]
        public void Can_Run_Write_Command_With_Param()
        {
            var initializedContext = GetInitializedContext();

            var command = @"
                CREATE TABLE mycats(id int not null, [name] nvarchar(max) not null, PRIMARY KEY(id));
                INSERT INTO mycats(id, [name]) VALUES (@idFuchur, @nameFuchur), (@idFenja, @nameFenja);
                SELECT * FROM mycats;";
            var param = new { idFuchur = 1, nameFuchur = "Fuchur the Devil", idFenja = 2, nameFenja = "Fenja the Goddess of sleep" };

            var result = initializedContext.RunCommand<dynamic>(command: command, param: param, mode: CommandMode.Write);

            var dataFuchur = result.ElementAt(0);
            var dataFenja = result.ElementAt(1);

            Assert.AreEqual(dataFuchur.id, 1);
            Assert.AreEqual(dataFuchur.name, "Fuchur the Devil");
            Assert.AreEqual(dataFenja.id, 2);
            Assert.AreEqual(dataFenja.name, "Fenja the Goddess of sleep");
        }

        [Test]
        public void Can_Set_Version_After_Init()
        {
            var validContext = GetValidContext();
            var expectedVersion = new SqlVersion(0, 0, 0);

            validContext.Init();

            var currentVersion = validContext.GetInstalledVersion().GetVersion();

            currentVersion.ShouldBeEquivalentTo(expectedVersion);
        }

        [Test]
        public void CanConnect_Returns_False_If_Not_Valid()
        {
            var invalidContext = new SqlContext("somethingInvalid", "zzzzyyy");

            Assert.IsFalse(invalidContext.CanConnect());
        }

        [Test]
        public void CanConnect_Returns_True_If_Valid()
        {
            var validContext = GetValidContext();

            Assert.IsTrue(validContext.CanConnect());
        }

        [Test]
        public void Cannot_Execute_Multiple_With_Same_Version()
        {
            var initializedContext = GetInitializedContext();

            var script1 = new SqlScript("script_1",
                $@"SELECT 1;",
                new SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            var script2 = new SqlScript("script_2",
                $@"SELECT 2;",
                new SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            initializedContext.ExecuteScript(script1);
            Assert.Throws<LowerOrEqualVersionException>(() => initializedContext.ExecuteScript(script2));
        }

        [Test]
        public void Cannot_Find_DB()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }

            Assert.IsFalse(validContext.Exists());
        }

        [Test]
        public void DeleteCommand_Can_Build_Command()
        {
            string schema = "core";
            string table = "typeCs";
            string whereClause = "id = @Id";

            var validContext = GetValidContext();
            validContext.RegisterModelDescription(SqlModelDescription<TypeC>.Auto(schema, table));

            var command = validContext.DeleteCommand<TypeC>(whereClause);

            command.Should().BeEquivalentTo($"DELETE FROM [{schema}].[{table}] WHERE {whereClause};");
        }

        [Test]
        public void DeleteCommand_Throws_On_Empty_WhereClause()
        {
            var validContext = GetValidContext();

            Assert.Throws<ArgumentException>(() => validContext.DeleteCommand<TypeC>(null));
            Assert.Throws<ArgumentException>(() => validContext.DeleteCommand<TypeC>(string.Empty));
            Assert.Throws<ArgumentException>(() => validContext.DeleteCommand<TypeC>("    "));
        }

        [Test]
        public void GetModelDescription_Can_Find_ModelDescription()
        {
            var validContext = GetValidContext();

            var descriptionTypeA = SqlModelDescription<TypeA>.Auto("core", "typeAs");
            var descriptionTypeB = SqlModelDescription<TypeB>.Auto("core", "typeBs");

            validContext.RegisterModelDescription(descriptionTypeA);
            validContext.RegisterModelDescription(descriptionTypeB);

            validContext.GetModelDescription<TypeA>().ShouldBeEquivalentTo(descriptionTypeA);
            validContext.GetModelDescription<TypeB>().ShouldBeEquivalentTo(descriptionTypeB);
        }

        [Test]
        public void Init_Creates_Database()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }

            Assert.IsFalse(validContext.Exists());

            validContext.Init();

            Assert.IsTrue(validContext.Exists());
        }

        [Test]
        public void Init_Creates_Version_Table()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }

            Assert.IsFalse(validContext.VersionTableExists());

            validContext.Init();

            Assert.IsTrue(validContext.VersionTableExists());
        }

        [Test]
        public void InsertCommand_Can_Build_Command()
        {
            string tableName = "typeCs";
            string schema = "core";

            var validContext = GetValidContext();
            validContext.RegisterModelDescription(SqlModelDescription<TypeC>.Auto(schema, tableName));

            string command = validContext.InsertCommand<TypeC>();

            command.Should().Be($"INSERT INTO [{schema}].[{tableName}] ([integer], [name]) OUTPUT inserted.id VALUES (@Integer, @Name);");
        }

        [Test]
        public void RegisterModelScription_Can_Not_Register_Duplicate_ModelDescription()
        {
            var validContext = GetValidContext();

            validContext.RegisterModelDescription(SqlModelDescription<TypeA>.Auto("schema", "typeA"));

            Assert.Throws<TypeAlreadyRegisteredException>(
                () => validContext.RegisterModelDescription(SqlModelDescription<TypeA>.Auto("schema", "typeA")));
        }

        [Test]
        public void RegisterModelScription_Can_Register_Multiple_ModelDescription()
        {
            var validContext = GetValidContext();

            validContext.RegisterModelDescription(SqlModelDescription<TypeA>.Auto("schema", "typeA"));
            validContext.RegisterModelDescription(SqlModelDescription<TypeB>.Auto("schema", "typeB"));

            validContext.GetModelDescription<TypeA>().Should().NotBeNull();
            validContext.GetModelDescription<TypeB>().Should().NotBeNull();
        }

        [Test]
        public void RegisterModelScription_Can_Register_Single_ModelDescription()
        {
            var validContext = GetValidContext();

            validContext.RegisterModelDescription(SqlModelDescription<TypeA>.Auto("schema", "typeA"));

            validContext.GetModelDescription<TypeA>().Should().NotBeNull();
        }

        [Test]
        public void Returns_False_If_Database_Does_Not_Exist()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }

            Assert.IsFalse(validContext.Exists());

            Assert.IsFalse(validContext.VersionTableExists());
        }

        [Test]
        public void Returns_False_If_Login_Does_Not_Exist()
        {
            var validContext = GetValidContext();

            bool exists = validContext.LoginExists(Guid.NewGuid().ToString());

            Assert.IsFalse(exists);
        }

        [Test]
        public void Returns_False_If_Version_Table_Does_Not_Exist()
        {
            var validContext = GetValidContext();

            bool exists = validContext.VersionTableExists();

            Assert.IsFalse(exists);
        }

        [Test]
        public void Returns_Null_If_No_Version_Installed()
        {
            var validContext = GetValidContext();
            SqlVersion version = null;

            version = validContext.GetInstalledVersion();

            Assert.IsNull(version);
        }

        [Test]
        public void Returns_True_If_Login_Exists()
        {
            var validContext = GetValidContext();
            string username = $"user_{Guid.NewGuid().ToString().Substring(0, 8)}";
            string password = Guid.NewGuid().ToString();

            validContext.CreateLogin(username, password);
            bool loginExists = validContext.LoginExists(username);
            validContext.RemoveLogin(username);

            Assert.IsTrue(loginExists);
        }

        [Test]
        public void Returns_True_If_Version_Table_Exists()
        {
            var validContext = GetValidContext();
            validContext.CreateVersionTable();

            bool exists = validContext.VersionTableExists();

            Assert.True(exists);
        }

        [Test]
        public void SelectCommand_Can_Build_Command()
        {
            string schema = "core";
            string tableName = "typeCs";

            var validContext = GetValidContext();
            validContext.RegisterModelDescription(SqlModelDescription<TypeC>.Auto(schema, tableName));

            string command = validContext.SelectCommand<TypeC>();

            command.Should().Be($"SELECT * FROM [{schema}].[{tableName}];");
        }

        [Test]
        public void SelectCommand_Can_Build_Command_With_Where_Clause()
        {
            string schema = "core";
            string tableName = "typeCs";

            var validContext = GetValidContext();
            validContext.RegisterModelDescription(SqlModelDescription<TypeC>.Auto(schema, tableName));

            string command = validContext.SelectCommand<TypeC>("Id = @Id");

            command.Should().Be($"SELECT * FROM [{schema}].[{tableName}] WHERE Id = @Id;");
        }

        [Test]
        public void SelectCommand_Throws_On_Unregistered_Type()
        {
            Assert.Throws<ModelDescriptionNotFoundException>(()
                => GetValidContext().SelectCommand<TypeA>());
        }

        [SetUp]
        public void Setup()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }
            validContext.CreateDatabase();
        }

        [TearDown]
        public void Teardown()
        {
            var validContext = GetValidContext();
            if (validContext.Exists())
            {
                validContext.DropDatabase();
            }
        }

        [Test]
        public void UpdateCommand_Can_Build_Command()
        {
            string schema = "core";
            string table = "typeCs";
            string whereClause = "id = @Id";

            var validContext = GetValidContext();
            validContext.RegisterModelDescription(SqlModelDescription<TypeC>.Auto(schema, table));

            var command = validContext.UpdateCommand<TypeC>(whereClause);

            command.Should().Be($"UPDATE [{schema}].[{table}] SET [integer] = @Integer, [name] = @Name WHERE {whereClause};");
        }

        [Test]
        public void UpdateCommand_Throws_On_Empty_WhereClause()
        {
            var validContext = GetValidContext();
            Assert.Throws<ArgumentException>(() => validContext.UpdateCommand<TypeC>(null));
            Assert.Throws<ArgumentException>(() => validContext.UpdateCommand<TypeC>(string.Empty));
            Assert.Throws<ArgumentException>(() => validContext.UpdateCommand<TypeC>("    "));
        }
    }

    public class TypeA
    {
        public string Name { get; set; }
    }

    public class TypeB
    {
        public int Integer { get; set; }
    }

    public class TypeC
    {
        public Guid Id { get; set; }

        public int Integer { get; set; }
        public string Name { get; set; }

        public string Readonly
        {
            get
            { return "readonly"; }
        }

        private int Hidden { get; set; }
    }
}