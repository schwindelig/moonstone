using FluentAssertions;
using moonstone.core.services;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace moonstone.services.tests
{
    [TestFixture]
    public class EnvironmentServiceTest
    {
        private const string DEFAULT_CULTURE = "en-US";
        private const string NEW_CULTURE = "de-CH";

        public IEnvironmentService EnvironmentService { get; set; }

        [SetUp]
        public void _SetUp()
        {
            var ctx = Provider.GetSqlContext();
            var repoHub = Provider.GetRepositoryHub(ctx);
            var serviceHub = Provider.GetServiceHub(repoHub);
            this.EnvironmentService = serviceHub.EnvironmentService;
        }

        [Test]
        [SetCulture(DEFAULT_CULTURE)]
        [SetUICulture(DEFAULT_CULTURE)]
        public void SetCulture_ChangesCulture()
        {
            this.EnvironmentService.SetCulture(NEW_CULTURE);

            Thread.CurrentThread.CurrentCulture.Name.ShouldBeEquivalentTo(NEW_CULTURE);
            Thread.CurrentThread.CurrentUICulture.Name.ShouldBeEquivalentTo(NEW_CULTURE);
        }

        [Test]
        [SetCulture(DEFAULT_CULTURE)]
        [SetUICulture(DEFAULT_CULTURE)]
        public void SetCulture_Throws_OnEmptyCulture()
        {
            Assert.Throws<ArgumentException>(
                () => this.EnvironmentService.SetCulture(string.Empty));

            Assert.Throws<ArgumentException>(
                () => this.EnvironmentService.SetCulture(null));
        }
    }
}