using FluentAssertions;
using moonstone.sql.context;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.tests.context
{
    public class Animal
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Cat : Animal
    {
        public bool IsGrumpy { get { return true; } }

        public string Title { get; set; }
    }

    [TestFixture]
    public class SqlModelDescriptionTest
    {
        [Test]
        public void Auto_Ignores_ReadOnly_Properties()
        {
            var description = SqlModelDescription<Cat>.Auto("schema", "cats");

            description.Property(p => p.IsGrumpy).Should().BeNull();
        }

        [Test]
        public void Auto_Registers_Inherited_Property()
        {
            var description = SqlModelDescription<Cat>.Auto("schema", "cats");

            description.Property(c => c.Id).Should().NotBeNull();
            description.Property(c => c.Name).Should().NotBeNull();
        }

        [Test]
        public void Auto_Registers_Simple_Property()
        {
            var description = SqlModelDescription<Cat>.Auto("core", "cats");

            description.Property(c => c.Title).Should().NotBeNull();
        }

        [Test]
        public void GetProperty_Finds_Property()
        {
            var description = SqlModelDescription<Cat>.Auto("core", "cats");

            description.Property(p => p.Name).Should().NotBeNull();
        }
    }
}