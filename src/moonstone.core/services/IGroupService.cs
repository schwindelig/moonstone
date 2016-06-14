using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface IGroupService
    {
        void AddUserToGroup(Guid userId, Guid groupId);

        Group CreateGroup(Group group);

        Group GetGroupById(Guid groupId);

        IEnumerable<Group> GetGroupsForUser(Guid userId);

        IEnumerable<User> GetUsersForGroup(Guid groupId);
    }
}