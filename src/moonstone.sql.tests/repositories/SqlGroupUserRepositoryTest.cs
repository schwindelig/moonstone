using FluentAssertions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.tests.repositories
{
    public class SqlGroupUserRepositoryTest
    {
        public IGroupRepository GroupRepository { get; set; }
        public IGroupUserRepository GroupUserRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public Group _CreateNewGroup(Guid creatorUserId)
        {
            return this.GroupRepository.GetById(
                this.GroupRepository.Create(TestProvider.GetNewGroup(creatorUserId)));
        }

        public User _CreateNewUser()
        {
            return this.UserRepository.GetById(
                this.UserRepository.Create(TestProvider.GetNewUser()));
        }

        [SetUp]
        public void _SetUp()
        {
            var ctx = TestProvider.GetSqlContext();
            var repoHub = TestProvider.GetRepositoryHub(ctx);

            this.UserRepository = repoHub.UserRepository;
            this.GroupRepository = repoHub.GroupRepository;
            this.GroupUserRepository = repoHub.GroupUserRepository;
        }

        [Test]
        public void Create_CanCreateGroupUser()
        {
            var user1 = TestProvider.GetNewUser(); // creator
            var user2 = TestProvider.GetNewUser(); // assigned user

            user1 = this.UserRepository.GetById(this.UserRepository.Create(user1));
            user2 = this.UserRepository.GetById(this.UserRepository.Create(user2));

            var group = TestProvider.GetNewGroup(user1.Id);
            group = this.GroupRepository.GetById(this.GroupRepository.Create(group));

            var groupUser = new GroupUser { GroupId = group.Id, UserId = user2.Id };

            this.GroupUserRepository.Create(groupUser);

            var forGroup = this.GroupUserRepository.GetForGroup(group.Id);
            var forUser = this.GroupUserRepository.GetForUser(user2.Id);

            forGroup.Should().NotBeNull();
            forUser.Should().NotBeNull();
            forGroup.ShouldBeEquivalentTo(forUser);
        }

        [Test]
        public void GetForGroup_CanFindGroupsForUser()
        {
            var userCreator = this._CreateNewUser();
            var user1 = this._CreateNewUser();
            var user2 = this._CreateNewUser();
            var user3 = this._CreateNewUser();

            var group1 = this._CreateNewGroup(userCreator.Id);
            var group2 = this._CreateNewGroup(userCreator.Id);
            var group3 = this._CreateNewGroup(userCreator.Id);

            var assign1_1 = new GroupUser { GroupId = group1.Id, UserId = user1.Id };
            var assign2_1 = new GroupUser { GroupId = group1.Id, UserId = user2.Id };
            var assign2_2 = new GroupUser { GroupId = group2.Id, UserId = user2.Id };
            var assign3_1 = new GroupUser { GroupId = group1.Id, UserId = user3.Id };
            var assign3_2 = new GroupUser { GroupId = group2.Id, UserId = user3.Id };
            var assign3_3 = new GroupUser { GroupId = group3.Id, UserId = user3.Id };

            // User 1
            this.GroupUserRepository.Create(assign1_1);

            // User 2
            this.GroupUserRepository.Create(assign2_1);
            this.GroupUserRepository.Create(assign2_2);

            // User 3
            this.GroupUserRepository.Create(assign3_1);
            this.GroupUserRepository.Create(assign3_2);
            this.GroupUserRepository.Create(assign3_3);

            var groupsForUser1 = this.GroupUserRepository.GetForUser(user1.Id);
            var groupsForUser2 = this.GroupUserRepository.GetForUser(user2.Id);
            var groupsForUser3 = this.GroupUserRepository.GetForUser(user3.Id);

            groupsForUser1.Should().HaveCount(1);
            groupsForUser1.ShouldAllBeEquivalentTo(new GroupUser[] { assign1_1 });

            groupsForUser2.Should().HaveCount(2);
            groupsForUser2.ShouldAllBeEquivalentTo(new GroupUser[] { assign2_1, assign2_2 });

            groupsForUser3.Should().HaveCount(3);
            groupsForUser3.ShouldAllBeEquivalentTo(new GroupUser[] { assign3_1, assign3_2, assign3_3 });
        }

        [Test]
        public void GetForUser_CanFindGroupsForUser()
        {
            var userCreator = this._CreateNewUser();
            var user1 = this._CreateNewUser();
            var user2 = this._CreateNewUser();
            var user3 = this._CreateNewUser();

            var group1 = this._CreateNewGroup(userCreator.Id);
            var group2 = this._CreateNewGroup(userCreator.Id);
            var group3 = this._CreateNewGroup(userCreator.Id);

            var assign1_1 = new GroupUser { GroupId = group1.Id, UserId = user1.Id };
            var assign2_1 = new GroupUser { GroupId = group1.Id, UserId = user2.Id };
            var assign2_2 = new GroupUser { GroupId = group2.Id, UserId = user2.Id };
            var assign3_1 = new GroupUser { GroupId = group1.Id, UserId = user3.Id };
            var assign3_2 = new GroupUser { GroupId = group2.Id, UserId = user3.Id };
            var assign3_3 = new GroupUser { GroupId = group3.Id, UserId = user3.Id };

            // User 1
            this.GroupUserRepository.Create(assign1_1);

            // User 2
            this.GroupUserRepository.Create(assign2_1);
            this.GroupUserRepository.Create(assign2_2);

            // User 3
            this.GroupUserRepository.Create(assign3_1);
            this.GroupUserRepository.Create(assign3_2);
            this.GroupUserRepository.Create(assign3_3);

            var usersForGroup1 = this.GroupUserRepository.GetForGroup(group1.Id);
            var usersForGroup2 = this.GroupUserRepository.GetForGroup(group2.Id);
            var usersForGroup3 = this.GroupUserRepository.GetForGroup(group3.Id);

            usersForGroup1.Should().HaveCount(3);
            usersForGroup1.ShouldAllBeEquivalentTo(new GroupUser[] { assign1_1, assign2_1, assign3_1 });

            usersForGroup2.Should().HaveCount(2);
            usersForGroup2.ShouldAllBeEquivalentTo(new GroupUser[] { assign2_2, assign3_2 });

            usersForGroup3.Should().HaveCount(1);
            usersForGroup3.ShouldAllBeEquivalentTo(new GroupUser[] { assign3_3 });
        }
    }
}