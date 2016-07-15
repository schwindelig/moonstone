using moonstone.sql.context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace moonstone.dbmanage
{
    internal enum Action
    {
        Init = 0,
        Nuke,
        Drop,
        Cls,
        Clear,
        Version,
        Ver,
        V,
        Exist,
        Exists,
        Reset,
        Update,
        Info,
        Help,
        Exit
    }

    internal class Program
    {
        protected static string Database { get; set; }

        protected static string Server { get; set; }

        private static void ActionLoop()
        {
            var action = RequestAction();

            switch (action)
            {
                case Action.Init:
                    Init();
                    break;

                case Action.Nuke:
                case Action.Drop:
                    Drop();
                    break;

                case Action.Cls:
                case Action.Clear:
                    Console.Clear();
                    break;

                case Action.Reset:
                    RequestConnectionDetails();
                    PrintInfo();
                    break;

                case Action.Version:
                case Action.Ver:
                case Action.V:
                    PrintVersion();
                    break;

                case Action.Exists:
                case Action.Exist:
                    PrintExists();
                    break;

                case Action.Update:
                    RunUpdate();
                    break;

                case Action.Info:
                    PrintInfo();
                    break;

                case Action.Help:
                    PrintHelp();
                    break;

                case Action.Exit:
                    return;
            }

            ActionLoop();
        }

        private static void Drop()
        {
            var context = GetContext();

            try
            {
                if (context.Exists())
                {
                    context.DropDatabase();
                    Console.WriteLine($"Database dropped.");
                }
                else
                {
                    Console.WriteLine($"Database does not exists.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to drop database: {e.Message}");
            }
        }

        private static SqlContext GetContext()
        {
            return new SqlContext(Database, Server);
        }

        private static void Init()
        {
            var context = GetContext();

            try
            {
                if (!context.Exists())
                {
                    context.Init();
                    Console.WriteLine($"Database initialized.");
                }
                else
                {
                    Console.WriteLine($"Database already exists.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to initialize database: {e.Message}");
            }
        }

        private static void Main(string[] args)
        {
            PrintSeparator();
            Console.WriteLine("Type help after connecting to see the list of available commands");
            PrintSeparator();
            Console.WriteLine();

            RequestConnectionDetails();
            PrintInfo();

            ActionLoop();
        }

        private static void PrintExists()
        {
            var context = GetContext();
            Console.WriteLine(context.Exists());
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Available commands (including aliases):");
            foreach (var name in Enum.GetNames(typeof(Action)).OrderBy(n => n))
            {
                Console.WriteLine($"{name.ToLower()}");
            }
            Console.WriteLine();
        }

        private static void PrintInfo()
        {
            PrintSeparator();
            Console.WriteLine("Connection info:");
            PrintSeparator();

            var context = GetContext();

            Console.WriteLine($"Server: {context.ServerAddress}");
            Console.WriteLine($"Database: {context.DatabaseName}");

            if (context.CanConnect())
            {
                Console.WriteLine("+ Can connect to server");
                if (context.Exists())
                {
                    Console.WriteLine($"+ Database exists");
                    if (context.VersionTableExists())
                    {
                        Console.WriteLine($"+ Version table exists");
                        var currentVersion = context.GetInstalledVersion();
                        if (currentVersion != null)
                        {
                            Console.WriteLine($"+ Current version is {currentVersion}");
                        }
                        else
                        {
                            Console.WriteLine($"- No version found");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"- Version table does not exist");
                    }
                }
                else
                {
                    Console.WriteLine("- Database does not exist");
                }
            }
            else
            {
                Console.WriteLine($"- Can not connect to server");
            }

            PrintSeparator();
        }

        private static void PrintSeparator()
        {
            for (int i = 0; i < Console.BufferWidth / 2; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        private static void PrintVersion()
        {
            try
            {
                var context = GetContext();
                if (context.Exists())
                {
                    Console.WriteLine($"Current version is: {context.GetInstalledVersion()}");
                }
                else
                {
                    Console.WriteLine("Database does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to get version: {e.Message}");
            }
        }

        private static Action RequestAction()
        {
            string action = Console.ReadLine();

            foreach (var name in Enum.GetNames(typeof(Action)))
            {
                if (action.Trim().ToLower() == name.ToLower())
                {
                    return (Action)Enum.Parse(typeof(Action), name);
                }
            }

            Console.WriteLine("Invalid action.");
            return RequestAction();
        }

        private static void RequestConnectionDetails()
        {
            Console.Write("Enter Server address: ");
            Server = Console.ReadLine();

            Console.Write("Enter DB Name: ");
            Database = Console.ReadLine();
        }

        private static bool RequestContinueConfirmation()
        {
            Console.Write("Continue? (yes/no) ");
            var inputContinue = Console.ReadLine().Trim().ToLower();
            return (inputContinue == "yes" || inputContinue == "y");
        }

        private static DirectoryInfo RequestDirectory()
        {
            Console.Write($"Directory: ");
            string path = Console.ReadLine();
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path);
            }
            else
            {
                Console.WriteLine($"Invalid directory");
                return RequestDirectory();
            }
        }

        private static void RunUpdate()
        {
            var directory = RequestDirectory();
            var files = directory.GetFiles("*.sql");

            Console.WriteLine($"Found {files.Count()} files:");
            foreach (var file in files)
            {
                Console.WriteLine($"- {file.Name}");
            }

            if (RequestContinueConfirmation())
            {
                RunUpdate(files);
            }
        }

        private static void RunUpdate(IEnumerable<FileInfo> files)
        {
            var scripts = new List<SqlScript>();

            foreach (var file in files)
            {
                try
                {
                    scripts.Add(SqlScript.FromFile(file.Name, file.FullName, true, true));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to parse file {file.FullName}: {e.Message}");
                    Console.WriteLine($"Canceling update.");
                    return;
                }
            }

            Console.WriteLine($"Failed parsed successfully");

            var context = GetContext();
            scripts = scripts
                .Where(s => s.Version.CompareTo(context.GetInstalledVersion().GetVersion()) == 1)
                .OrderBy(s => s.Version.Major)
                .ThenBy(s => s.Version.Minor)
                .ThenBy(s => s.Version.Revision).ToList();

            Console.WriteLine("Filtered and ordered scripts. Order is: ");
            foreach (var script in scripts)
            {
                Console.WriteLine($"- {script.Version.ToString()} ({script.Name})");
            }

            if (RequestContinueConfirmation())
            {
                foreach (var script in scripts)
                {
                    try
                    {
                        Console.Write($"Executing {script.Version.ToString()} {script.Name} ... ");

                        context.ExecuteScript(script);

                        Console.WriteLine("Success");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to execute script {script.Name}: {e.ToString()}");
                        Console.WriteLine("Canceling update");
                        return;
                    }
                }

                Console.WriteLine($"Update completed successfully. Current version is: {context.GetInstalledVersion()}");
            }
        }
    }
}