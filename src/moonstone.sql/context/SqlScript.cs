using moonstone.core.exceptions;
using System;
using System.IO;
using System.Linq;

namespace moonstone.sql.context
{
    public class SqlScript
    {
        public string Command { get; set; }
        public string Name { get; set; }
        public bool UseSpecifiedDatabase { get; set; }
        public bool UseTransaction { get; set; }
        public SqlVersion Version { get; set; }

        public SqlScript(string name, string command, SqlVersion version, bool useSpecifiedDatabase, bool useTransaction)
        {
            this.Name = name;
            this.Command = command;
            this.Version = version;
            this.UseSpecifiedDatabase = useSpecifiedDatabase;
            this.UseTransaction = useTransaction;
        }

        public static SqlScript FromContent(string name, string content, SqlVersion version, bool useSpecifiedDatabase, bool useTransaction)
        {
            return new SqlScript(name, content, version, useSpecifiedDatabase, useTransaction);
        }

        public static SqlScript FromFile(string name, string path, SqlVersion version, bool useSpecifiedDatabase, bool useTransaction)
        {
            var content = ReadContent(path);
            return FromContent(name, content, version, useSpecifiedDatabase, useTransaction);
        }

        public static SqlScript FromFile(string name, string path, bool useSpecifiedDatabase, bool useTransaction)
        {
            var content = ReadContent(path);
            var version = ParseVersion(content);

            return FromContent(name, content, version, useSpecifiedDatabase, useTransaction);
        }

        protected static SqlVersion ParseVersion(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    var firstLine = content.Split(Environment.NewLine.ToCharArray()).First();
                    firstLine = firstLine.Replace("--", string.Empty);
                    firstLine = firstLine.Trim();
                    var versionNumbers = firstLine.Split('.').Select(v => int.Parse(v)).ToArray();

                    return new SqlVersion(versionNumbers[0], versionNumbers[1], versionNumbers[2]);
                }
                catch (Exception e)
                {
                    throw new ParseException(
                        $"Failed to parse file.", e);
                }
            }
            else
            {
                throw new EmptyFileException(
                       $"File is empty.");
            }
        }

        protected static string ReadContent(string path)
        {
            string content;
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                throw new ReadFileException(
                    $"Failed to read contents from {path}", e);
            }

            return content;
        }
    }
}