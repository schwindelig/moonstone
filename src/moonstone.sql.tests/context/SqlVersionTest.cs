using FluentAssertions;
using NUnit.Framework;
using System;

namespace moonstone.sql.tests.context
{
    [TestFixture]
    public class SqlVersionTest
    {
        [Test]
        public void Can_Compare()
        {
            var low = new Version(0, 0, 9);
            var high = new Version(1, 1, 1);

            low.Should().BeLessThan(high);
        }
    }
}