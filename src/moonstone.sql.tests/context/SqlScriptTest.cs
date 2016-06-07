using moonstone.sql.context;
using NUnit.Framework;
using System.IO;

namespace moonstone.sql.tests.context
{
    [TestFixture]
    public class SqlScriptTest
    {
        [Test]
        public void Can_Init_From_File()
        {
            var path = "test_init_from_file.sql";
            var command = "SELECT 42";
            var version = new SqlVersion(1, 0, 0);

            File.WriteAllText(path, command);
            SqlScript script = null;

            script = SqlScript.FromFile("test", path, version, useSpecifiedDatabase: true, useTransaction: false);

            Assert.AreEqual(command, script.Command);
            Assert.AreEqual(version, script.Version);
        }

        [Test]
        public void Can_Parse_Version()
        {
            var path = "test_parse_version.sql";
            var content = @"-- 3.2.34
                            SELECT * FROM test;";

            File.WriteAllText(path, content);

            var script = SqlScript.FromFile("test_script", path, useSpecifiedDatabase: true, useTransaction: true);

            Assert.That(script.Version.CompareTo(new SqlVersion(3, 2, 34)) == 0);
        }
    }
}