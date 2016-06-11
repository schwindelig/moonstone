using FluentAssertions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.sql.repositories;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.tests.repositories
{
    [TestFixture]
    public class SqlGroupRepositoryTest
    {
        public User Creator { get; set; }
        public IGroupRepository GroupRepository { get; set; }

        [SetUp]
        public void _SetUp()
        {
            AssertionOptions.AssertEquivalencyUsing(options =>
              options.Using<DateTime>(durr => durr.Subject.Should().BeCloseTo(durr.Expectation)).WhenTypeIs<DateTime>()
            );

            var ctx = TestProvider.GetSqlContext();
            var repos = TestProvider.GetRepositoryHub(ctx);

            this.GroupRepository = repos.GroupRepository;

            this.Creator = repos.UserRepository.GetById(
                repos.UserRepository.Create(TestProvider.GetNewUser()));
        }

        [Test]
        public void Create_CanCreateGroup()
        {
            var userId = this.Creator.Id;
            var group = TestProvider.GetNewGroup(userId);

            var groupId = this.GroupRepository.Create(group);
            group = this.GroupRepository.GetById(groupId);

            groupId.Should().NotBe(default(Guid));
            group.Should().NotBeNull();
        }

        [Test]
        public void GetById_CanFindGroup()
        {
            var group = TestProvider.GetNewGroup(this.Creator.Id);
            group.Id = this.GroupRepository.Create(group);

            var newGroup = this.GroupRepository.GetById(group.Id);

            newGroup.ShouldBeEquivalentTo(group);
        }
    }
}