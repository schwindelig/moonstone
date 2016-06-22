using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(RepositoryHub repoHub)
            : base(repoHub)
        {
        }

        public void AddUserToGroup(Guid userId, Guid groupId)
        {
            this.Repositories.GroupUserRepository.Create(new GroupUser { UserId = userId, GroupId = groupId });
        }

        public Group CreateGroup(Group group)
        {
            var groupId = this.Repositories.GroupRepository.Create(group);
            group = this.GetGroupById(groupId);

            this.AddUserToGroup(group.CreateUserId, group.Id);

            return group;
        }

        public Group GetGroupById(Guid groupId)
        {
            return this.Repositories.GroupRepository.GetById(groupId);
        }

        public IEnumerable<Group> GetGroupsForUser(Guid userId)
        {
            var assigns = this.Repositories.GroupUserRepository.GetForUser(userId);

            return assigns.Select(a => this.Repositories.GroupRepository.GetById(a.GroupId));
        }

        public IEnumerable<User> GetUsersForGroup(Guid groupId)
        {
            var assigns = this.Repositories.GroupUserRepository.GetForGroup(groupId);

            return assigns.Select(a => this.Repositories.UserRepository.GetById(a.UserId));
        }

        public bool IsUserInGroup(Guid userId, Guid groupId)
        {
            var groups = this.GetGroupsForUser(userId);

            return groups.Any(g => g.Id == groupId);
        }
    }
}