using FluentAssertions;
using moonstone.core.models;
using moonstone.core.services;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services.tests
{
    [TestFixture]
    public class GroupServiceTest
    {
        public User Creator { get; set; }
        public IGroupService GroupService { get; set; }

        [SetUp]
        public void _SetUp()
        {
            AssertionOptions.AssertEquivalencyUsing(options =>
              options.Using<DateTime>(durr => durr.Subject.Should().BeCloseTo(durr.Expectation)).WhenTypeIs<DateTime>()
            );

            var ctx = TestProvider.GetSqlContext();
            var repos = TestProvider.GetRepositoryHub(ctx);
            var serviceHub = TestProvider.GetServiceHub(repos);
            this.GroupService = serviceHub.GroupService;

            this.Creator = repos.UserRepository.GetById(
                repos.UserRepository.Create(TestProvider.GetNewUser()));
        }

        [Test]
        public void Create_CanCreateGroup()
        {
            var group = this.GroupService.CreateGroup(
                TestProvider.GetNewGroup(this.Creator.Id));

            group.Should().NotBeNull();
        }

        [Test]
        public void GetById_CanFindGroup()
        {
            var group = TestProvider.GetNewGroup(this.Creator.Id);
            group = this.GroupService.CreateGroup(group);

            var result = this.GroupService.GetGroupById(group.Id);

            result.ShouldBeEquivalentTo(group);
        }
    }
}