using FluentAssertions;
using moonstone.core.i18n;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace moonstone.core.tests
{
    [TestFixture]
    public class CultureNinjaTest
    {
        private const string newCulture = "de-CH";
        private const string originalCulture = "en-US";

        [Test]
        public void SetCulture_CanReadDefaultResource()
        {
            TestResources.String_HelloWorld.ShouldBeEquivalentTo("Hello World");

            CultureNinja culture = new CultureNinja();
            culture.SetCulture("de");

            TestResources.String_HelloWorld.ShouldBeEquivalentTo("Hallo Welt");
        }

        [Test]
        [SetCulture(originalCulture)]
        [SetUICulture(originalCulture)]
        public void SetCulture_ChangesCulture()
        {
            Thread.CurrentThread.CurrentCulture.Name.ShouldBeEquivalentTo(originalCulture);
            Thread.CurrentThread.CurrentUICulture.Name.ShouldBeEquivalentTo(originalCulture);

            CultureNinja culture = new CultureNinja();
            culture.SetCulture(newCulture);

            Thread.CurrentThread.CurrentCulture.Name.ShouldBeEquivalentTo(newCulture);
            Thread.CurrentThread.CurrentUICulture.Name.ShouldBeEquivalentTo(newCulture);
        }
    }
}