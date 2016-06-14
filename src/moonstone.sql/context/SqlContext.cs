using Dapper;
using moonstone.core.exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace moonstone.sql.context
{
    public enum CommandMode
    {
        Read,
        Write
    }

    public class SqlContext
    {
        protected const string CORE_SCHEMA = "core";
        protected const string USERS_TABLE = "users";
        protected const string VERSION_TABLE = "db_version";

        /// <summary>
        /// The name of the database
        /// </summary>
        public string DatabaseName { get; protected set; }

        /// <summary>
        /// Returns Flag indicating if integrated security should be should be used.
        /// </summary>
        public bool IntegratedSecurity
        {
            get
            {
                return this.UserID == null && this.Password == null;
            }
        }

        /// <summary>
        /// The password that will be used to connect to the database.
        /// Set to null if integrated security should be used.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The connection string the the server
        /// </summary>
        public string ServerAddress { get; protected set; }

        /// <summary>
        /// The user id that will be used to connect to the database.
        /// Set to null if integrated security should be used.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Holds all ModelDescriptions. Use RegisterModelDescription to add items
        /// </summary>
        protected ICollection<SqlModelDescription> ModelDescriptions { get; set; }

        /// <summary>
        /// Initializes a new context. Uses integrated security for connection.
        /// </summary>
        /// <param name="serverAddress">Adress of the server hosting the database</param>
        /// <param name="databaseName">Name of the database</param>
        public SqlContext(string databaseName, string serverAddress)
            : this(databaseName, serverAddress, null, null)
        {
        }

        /// <summary>
        /// Initializes a new context.
        /// </summary>
        /// <param name="databaseName">Adress of the server hosting the database</param>
        /// <param name="serverAddress">Name of the datase</param>
        /// <param name="userId">SQL user ID</param>
        /// <param name="password">SQL password</param>
        public SqlContext(string databaseName, string serverAddress, string userId, string password)
        {
            this.DatabaseName = databaseName;
            this.ServerAddress = serverAddress;
            this.UserID = userId;
            this.Password = password;
            this.ModelDescriptions = new List<SqlModelDescription>();
        }

        /// <summary>
        /// Adds a record to the version table
        /// </summary>
        /// <param name="version">InstalledVersion to add</param>
        public void AddInstalledVersion(SqlInstalledVersion version)
        {
            this.CheckVersion(version);

            object param = null;
            var command = this.BuildAddInstalledVersionCommand(version, out param);

            using (var connection = this.OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Query(
                        sql: command,
                        param: param,
                        transaction: transaction);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();

                        throw new AddInstalledVersionException(
                            $"Failed to add installed version record", e);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the 'USE XYZ;' part to the command.
        /// </summary>
        /// <param name="command">The command without the 'USE XYZ' part</param>
        /// <param name="useSpecifiedDatabase">If true, the DatabaseName will be used, otherwise 'master'</param>
        /// <returns></returns>
        public string BuildCommand(string command, bool useSpecifiedDatabase)
        {
            string dbToUse = useSpecifiedDatabase ? this.DatabaseName : "master";
            command = this.ReplacePlaceholders(command);

            return $"USE {dbToUse}; {command}";
        }

        /// <summary>
        /// Checks if the connection can be opened.
        /// </summary>
        /// <returns>True if connection can be opened, otherwise false</returns>
        public bool CanConnect()
        {
            try
            {
                this.OpenConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if there is a newer version installed. Throws AddLowerVersionException if
        /// equal or higher version was found.
        /// </summary>
        /// <param name="installedVersionToCheck"></param>
        public void CheckVersion(SqlInstalledVersion installedVersionToCheck)
        {
            var currentInstalledVersion = this.GetInstalledVersion();
            if (currentInstalledVersion != null)
            {
                var currentVersion = currentInstalledVersion.GetVersion();
                var versionToCheck = installedVersionToCheck.GetVersion();

                if (currentVersion != null && currentVersion.CompareTo(versionToCheck) >= 0)
                {
                    throw new LowerOrEqualVersionException(
                        $"Attempt to add lower version. Current version is {currentVersion}");
                }
            }
        }

        /// <summary>
        /// Creates the database
        /// </summary>
        public void CreateDatabase()
        {
            var command = this.BuildCommand(
                $"CREATE DATABASE {this.DatabaseName}",
                false);

            using (var connection = this.OpenConnection())
            {
                try
                {
                    connection.Query(sql: command);
                }
                catch (Exception e)
                {
                    throw new CreateDatabaseException(
                        $"Failed to create database", e);
                }
            }
        }

        /// <summary>
        /// Creates a new login on the database server.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        public void CreateLogin(string username, string password)
        {
            var command = this.BuildCommand($"CREATE LOGIN {username} WITH PASSWORD = '{password}';", useSpecifiedDatabase: false);

            try
            {
                using (var connection = this.OpenConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            connection.Query(sql: command, param: null, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new CreateLoginException(
                    $"Failed to create login.", e);
            }
        }

        /// <summary>
        /// Attempts to create the version table.
        /// </summary>
        public void CreateVersionTable()
        {
            if (this.VersionTableExists())
            {
                throw new VersionTableAlreadyExistsException(
                    $"The version table '{VERSION_TABLE}' already exists in the database.");
            }

            var command = this.BuildCommand(
                $@"CREATE TABLE {VERSION_TABLE} (
                        major INT NOT NULL,
                        minor INT NOT NULL,
                        revision INT NOT NULL,
                        installDateUtc DATETIME2 NOT NULL,
                        PRIMARY KEY (major, minor, revision)
                    );", true);

            try
            {
                using (var connection = this.OpenConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            connection.Query(sql: command, param: null, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new CreateVersionTableException(
                    $"Failed to create version table '{VERSION_TABLE}'.", e);
            }
        }

        public string DeleteCommand<T>(string whereClause)
        {
            if (string.IsNullOrWhiteSpace(whereClause))
            {
                throw new ArgumentException(
                    $"whereCaluse must not be null or empty.", nameof(whereClause));
            }

            var modelDescription = this.GetModelDescription<T>();
            if (modelDescription != null)
            {
                string command = $"DELETE FROM [{modelDescription.Schema}].[{modelDescription.Table}] WHERE {whereClause};";

                return command;
            }
            else
            {
                throw new ModelDescriptionNotFoundException(
                    $"Model description for type {typeof(T).Name} was not found. Is the type registered?");
            }
        }

        /// <summary>
        /// Drops the database
        /// </summary>
        public void DropDatabase()
        {
            var command = this.BuildCommand($"DROP DATABASE {this.DatabaseName}", false);
            try
            {
                using (var connection = this.OpenConnection())
                {
                    connection.Query(sql: command, param: null);
                }
            }
            catch (Exception e)
            {
                throw new DropDatabaseException(
                    $"Failed to drop database", e);
            }
        }

        /// <summary>
        /// Drops the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="useSpecifiedDatabase">If true, the specified database will be used. Otherwise master.</param>
        public void DropTable(string tableName, bool useSpecifiedDatabase)
        {
            var command = this.BuildCommand($"DROP TABLE {tableName}", useSpecifiedDatabase: useSpecifiedDatabase);

            try
            {
                using (var connection = this.OpenConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            connection.Query(sql: command, param: null, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DropTableException(
                    $"Failed to drop table {tableName}", e);
            }
        }

        /// <summary>
        /// Executes the specified script on the database. If successfull, the installed version will be added
        /// </summary>
        /// <param name="script"></param>
        public void ExecuteScript(SqlScript script)
        {
            var version = new SqlInstalledVersion(
                script.Version.Major,
                script.Version.Minor,
                script.Version.Revision,
                DateTime.UtcNow);
            this.CheckVersion(version);

            object addVersionParam = null;
            var addVersionCommand = this.BuildAddInstalledVersionCommand(version, out addVersionParam);

            try
            {
                var command = this.BuildCommand(script.Command, useSpecifiedDatabase: script.UseSpecifiedDatabase);

                using (var connection = this.OpenConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            connection.Query(sql: command, param: null, transaction: transaction);
                            connection.Query(sql: addVersionCommand, param: addVersionParam, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ExecuteScriptException(
                    $"Failed to execute script {script.Name}.", e);
            }
        }

        /// <summary>
        /// Checks if the database exists
        /// </summary>
        /// <returns>True if exists, otherwise false</returns>
        public bool Exists()
        {
            try
            {
                var command = this.BuildCommand($"SELECT DB_ID('{this.DatabaseName}')", false);

                using (var connection = this.OpenConnection())
                {
                    var result = connection.Query<int?>(sql: command, param: null).SingleOrDefault();

                    return result.HasValue;
                }
            }
            catch (Exception e)
            {
                throw new MetaQueryException($"Failed to check if database exists.", e);
            }
        }

        /// <summary>
        /// Returns the connection string used to connect to the database
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            var connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = this.ServerAddress;
            connectionString.IntegratedSecurity = this.IntegratedSecurity;
            if (!this.IntegratedSecurity)
            {
                connectionString.UserID = this.UserID;
                connectionString.Password = this.Password;
            }

            var c = connectionString.ToString();
            return connectionString.ToString();
        }

        /// <summary>
        /// Returns the currently installed version
        /// </summary>
        /// <returns></returns>
        public SqlInstalledVersion GetInstalledVersion()
        {
            try
            {
                if (this.VersionTableExists())
                {
                    var command = this.BuildCommand(
                        $"SELECT TOP(1) * FROM {VERSION_TABLE} ORDER BY major DESC, minor DESC, revision DESC", true);

                    using (var connection = this.OpenConnection())
                    {
                        var result = connection.Query<SqlInstalledVersion>(sql: command).SingleOrDefault();
                        return result;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new RetreiveInstalledVersionException(
                    $"Failed to retrieve installed version.", e);
            }
        }

        /// <summary>
        /// Retreives the model description for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the model</typeparam>
        /// <returns></returns>
        public SqlModelDescription<T> GetModelDescription<T>()
        {
            return this.ModelDescriptions.SingleOrDefault(m => m is SqlModelDescription<T>) as SqlModelDescription<T>;
        }

        /// <summary>
        /// Returns the name of the users table
        /// </summary>
        /// <returns>Name of the users table</returns>
        public string GetUsersTableName()
        {
            return this.GetFQTableName(CORE_SCHEMA, USERS_TABLE);
        }

        /// <summary>
        /// Returns the name of the version table.
        /// </summary>
        /// <returns></returns>
        public string GetVersionTableName()
        {
            return VERSION_TABLE;
        }

        /// <summary>
        /// Initializes the database and sets current version to 0.0.0
        /// </summary>
        public void Init()
        {
            try
            {
                if (!this.Exists())
                {
                    this.CreateDatabase();
                }

                if (!this.VersionTableExists())
                {
                    this.CreateVersionTable();
                    this.AddInstalledVersion(new SqlInstalledVersion(0, 0, 0, DateTime.UtcNow));
                }
            }
            catch (Exception e)
            {
                throw new InitializeDatabaseException(
                    $"Failed to initialize the database", e);
            }
        }

        /// <summary>
        /// Returns the insert command for the specified type.
        /// </summary>
        /// <typeparam name="T">Type for insert</typeparam>
        /// <returns>Insert command for type</returns>
        public string InsertCommand<T>()
        {
            var modelDescription = this.GetModelDescription<T>();
            if (modelDescription != null)
            {
                var propertiesForInsert = modelDescription.Properties()
                    .Where(p => !p.IgnoreOnInsert);

                var fieldNames = propertiesForInsert.Select(p => $"[{p.FieldName}]");
                var propertyNames = propertiesForInsert.Select(p => $"@{p.PropertyName}");
                var pkFields = modelDescription.Properties().Where(p => p.IsPrimaryKey).Select(f => $"inserted.{f.FieldName}");

                string command = $"INSERT INTO [{modelDescription.Schema}].[{modelDescription.Table}] ({string.Join(", ", fieldNames)}) ";

                //command += $"OUTPUT inserted.Id "; // todo: this will break as soon as we have an pk-column which isn't named 'id'
                command += $"OUTPUT {string.Join(", ", pkFields)} ";

                command += $"VALUES ({string.Join(", ", propertyNames)});";

                return command;
            }
            else
            {
                throw new ModelDescriptionNotFoundException(
                    $"Model description for type {typeof(T).Name} was not found. Is the type registered?");
            }
        }

        /// <summary>
        /// Checks if the specified login exists on the db server.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        public bool LoginExists(string username)
        {
            var command = this.BuildCommand(
                $"SELECT COUNT(*) FROM master.dbo.syslogins WHERE name = '{username}'", false);

            try
            {
                using (var connection = this.OpenConnection())
                {
                    var result = connection.Query<int>(command).Single();
                    return result == 1;
                }
            }
            catch (Exception e)
            {
                throw new MetaQueryException(
                    $"Failed to check if login exists.", e);
            }
        }

        /// <summary>
        /// Returns an open SqlConnection
        /// </summary>
        /// <returns></returns>
        public SqlConnection OpenConnection()
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(this.GetConnectionString());
            }
            catch (Exception e)
            {
                throw new InitializeSqlConnectionException(
                    $"Failed to initialize sql connection", e);
            }

            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                throw new OpenConnectionException(
                    $"Failed to open connection.", e);
            }

            return connection;
        }

        /// <summary>
        /// Registers the specified model description.
        /// </summary>
        /// <typeparam name="T">Type of the model</typeparam>
        /// <param name="modelDescription">The actual model description</param>
        public void RegisterModelDescription<T>(SqlModelDescription<T> modelDescription)
        {
            if (this.GetModelDescription<T>() == null)
            {
                this.ModelDescriptions.Add(modelDescription);
            }
            else
            {
                throw new TypeAlreadyRegisteredException(
                    $"Type {typeof(T)} is already registerd.");
            }
        }

        /// <summary>
        /// Drops the specified login from the database server
        /// </summary>
        /// <param name="username">Username that should be removed</param>
        public void RemoveLogin(string username)
        {
            try
            {
                var command = this.BuildCommand(
                $"DROP LOGIN {username};", false);

                using (var connection = this.OpenConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            connection.Query(sql: command, param: null, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new RemoveLoginException(
                    $"Failed to remove login.", e);
            }
        }

        /// <summary>
        /// Runs the specified command on the database.
        /// </summary>
        /// <typeparam name="TReturn">Type to be returned.</typeparam>
        /// <param name="command">Command to run</param>
        /// <param name="param">Optional param object (can be anonymous)</param>
        /// <param name="mode">Specify 'Write' to run the command transactioned.</param>
        /// <returns>Result of the query</returns>
        public IEnumerable<TReturn> RunCommand<TReturn>(string command, object param, CommandMode mode)
        {
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            command = this.BuildCommand(command, useSpecifiedDatabase: true);

            try
            {
                connection = this.OpenConnection();
                if (mode == CommandMode.Write)
                {
                    transaction = connection.BeginTransaction();
                }

                var result = connection.Query<TReturn>(
                    sql: command,
                    param: param,
                    transaction: mode == CommandMode.Write ? transaction : null
                    );

                if (mode == CommandMode.Write)
                {
                    transaction.Commit();
                }

                return result;
            }
            catch (Exception e)
            {
                if (mode == CommandMode.Write && transaction != null)
                {
                    transaction.Rollback();
                }

                throw new QueryException(
                    $"Failed to execute query.", e);
            }
            finally
            {
                if (connection != null)
                {
                    if (mode == CommandMode.Write && transaction != null)
                    {
                        transaction.Dispose();
                    }
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Runs the specified command on the database.
        /// </summary>
        /// <typeparam name="TReturn">Type to be returned.</typeparam>
        /// <param name="command">Command to run</param>
        /// <param name="param">Optional param object (can be anonymous)</param>
        public void RunCommand(string command, object param, CommandMode mode)
        {
            this.RunCommand<dynamic>(command: command, param: param, mode: mode);
        }

        /// <summary>
        /// Creates the select command for the specified type.
        /// </summary>
        /// <typeparam name="T">Type for select command. Must be registered as ModelDescription.</typeparam>
        /// <returns></returns>
        public string SelectCommand<T>(string whereClause = null)
        {
            var modelDescription = this.GetModelDescription<T>();
            if (modelDescription != null)
            {
                var properties = modelDescription.Properties();

                var command =
                    $@"SELECT * FROM [{modelDescription.Schema}].[{modelDescription.Table}]";

                if (!string.IsNullOrWhiteSpace(whereClause))
                {
                    command = $"{command} WHERE {whereClause}";
                }

                command += ";";

                return command;
            }
            else
            {
                throw new ModelDescriptionNotFoundException(
                    $"Model description for type {typeof(T).Name} was not found. Is the type registered?");
            }
        }

        /// <summary>
        /// Returns the @@VERSION of the SQL Server
        /// </summary>
        /// <returns>Server Version, eg. Microsoft SQL Server 2014 - 12.0.2269.0 (X64)   Jun 10 2015 03:35:45   Copyright (c) Microsoft Corporation  Express Edition (64-bit) on Windows NT 6.3 ...</returns>
        public string ServerVersion()
        {
            try
            {
                var command = this.BuildCommand($"SELECT @@VERSION", false);

                using (var connection = this.OpenConnection())
                {
                    var result = connection.Query<string>(sql: command, param: null).SingleOrDefault();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new ReadDatabaseVerionException(
                    $"Failed to get database version", e);
            }
        }

        /// <summary>
        /// Checks if a table exists on the server.
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="useSpecifiedDatabase">If true, query uses the Database specified in the constructor.
        /// If false, master database will be used.</param>
        /// <returns>True if table was found. Otherwise false.</returns>
        public bool TableExists(string table, bool useSpecifiedDatabase)
        {
            try
            {
                var command = this.BuildCommand(
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}';",
                useSpecifiedDatabase);

                using (var connection = this.OpenConnection())
                {
                    var result = connection.Query<int>(sql: command).Single();
                    return result == 1;
                }
            }
            catch (Exception e)
            {
                throw new MetaQueryException(
                    $"Failed to check for existence of table '{table}'.", e);
            }
        }

        public string UpdateCommand<T>(string whereClause)
        {
            if (string.IsNullOrWhiteSpace(whereClause))
            {
                throw new ArgumentException(
                    $"whereClause cannot be null or empty", nameof(whereClause));
            }

            var modelDescription = this.GetModelDescription<T>();
            if (modelDescription != null)
            {
                var properties = modelDescription.Properties().Where(p => !p.IgnoreOnUpdate && !p.IsPrimaryKey);

                string assigns = string.Join(", ", properties.Select(p => $"[{p.FieldName}] = @{p.PropertyName}"));

                var command = $"UPDATE [{modelDescription.Schema}].[{modelDescription.Table}] "
                                + $"SET {assigns} "
                                + $"WHERE {whereClause};";

                return command;
            }
            else
            {
                throw new ModelDescriptionNotFoundException(
                    $"Model description for type {typeof(T).Name} was not found. Is the type registered?");
            }
        }

        /// <summary>
        /// Checks if the version table exists
        /// </summary>
        /// <returns>True if exists, otherwise false</returns>
        public bool VersionTableExists()
        {
            if (this.Exists())
            {
                return this.TableExists(this.GetVersionTableName(), useSpecifiedDatabase: true);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the command used to insert the installed version.
        /// </summary>
        /// <param name="version">Version to add</param>
        /// <param name="param">Used params</param>
        /// <returns>Insert command</returns>
        protected string BuildAddInstalledVersionCommand(SqlInstalledVersion version, out dynamic param)
        {
            var command = this.BuildCommand(
                $@"INSERT INTO {VERSION_TABLE} (major, minor, revision, installDateUtc)
                    VALUES (@major, @minor, @revision, @installDateUtc);", true);
            param = new
            {
                major = version.Major,
                minor = version.Minor,
                revision = version.Revision,
                installDateUtc = version.InstallDateUtc
            };

            return command;
        }

        /// <summary>
        /// Returns the fully qualified table name (eg. [database].[schema].[table])
        /// </summary>
        /// <param name="schema">Schema of the table</param>
        /// <param name="table">Name of the table</param>
        /// <returns></returns>
        protected string GetFQTableName(string schema, string table)
        {
            return $"[{this.DatabaseName}].[{schema}].[{table}]";
        }

        /// <summary>
        /// Replaces special placeholders in the defined command (eg. ::db::)
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>Command with actual values</returns>
        protected string ReplacePlaceholders(string command)
        {
            var placeholders = new Dictionary<string, string>()
            {
                { "::db::", this.DatabaseName }
            };

            foreach (var placeholder in placeholders)
            {
                command = command.Replace(placeholder.Key, placeholder.Value);
            }

            return command;
        }
    }
}