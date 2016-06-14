using FluentAssertions;
using moonstone.core.models;
using moonstone.core.repositories;
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
        public RepositoryHub Repositories { get; set; }

        [SetUp]
        public void _SetUp()
        {
            AssertionOptions.AssertEquivalencyUsing(options =>
              options.Using<DateTime>(durr => durr.Subject.Should().BeCloseTo(durr.Expectation)).WhenTypeIs<DateTime>()
            );

            var ctx = TestProvider.GetSqlContext();
            this.Repositories = TestProvider.GetRepositoryHub(ctx);
            var serviceHub = TestProvider.GetServiceHub(this.Repositories);
            this.GroupService = serviceHub.GroupService;

            this.Creator = this.Repositories.UserRepository.GetById(
                this.Repositories.UserRepository.Create(TestProvider.GetNewUser()));
        }

        [Test]
        public void AddUserToGroup_CanAddUserToGroup()
        {
            var userCreator = TestProvider.CreateNewUser(this.Repositories.UserRepository);

            var user = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, userCreator.Id);

            this.GroupService.AddUserToGroup(user.Id, group.Id);

            this.Repositories.GroupUserRepository.GetForGroup(group.Id)
                .Single().ShouldBeEquivalentTo(new GroupUser { GroupId = group.Id, UserId = user.Id });
        }

        [Test]
        public void Create_AddsCreatorToGroup()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);

            var group = this.GroupService.CreateGroup(TestProvider.GetNewGroup(creator.Id));

            this.GroupService.GetUsersForGroup(group.Id).Single().ShouldBeEquivalentTo(creator);
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

        [Test]
        public void GetGroupsForUser_CanFindGroupsForUser()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var user = TestProvider.CreateNewUser(this.Repositories.UserRepository);

            var group1 = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);
            var group2 = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);
            var trap = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);

            this.GroupService.AddUserToGroup(user.Id, group1.Id);
            this.GroupService.AddUserToGroup(user.Id, group2.Id);

            var groupsForUser = this.GroupService.GetGroupsForUser(user.Id);

            groupsForUser.ShouldAllBeEquivalentTo(new Group[] { group1, group2 });
        }

        [Test]
        public void GetUsersForGroup_CanFindUsersForGroup()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);

            var user1 = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var user2 = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var userTrap = TestProvider.CreateNewUser(this.Repositories.UserRepository);

            var group1 = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);
            var groupTrap = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);

            this.GroupService.AddUserToGroup(user1.Id, group1.Id);
            this.GroupService.AddUserToGroup(user2.Id, group1.Id);

            var userForGroup = this.GroupService.GetUsersForGroup(group1.Id);

            userForGroup.ShouldBeEquivalentTo(new User[] { user1, user2 });
        }
    }
}