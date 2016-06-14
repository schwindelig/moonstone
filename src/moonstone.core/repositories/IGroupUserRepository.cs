using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.repositories
{
    public interface IGroupUserRepository
    {
        void Create(GroupUser groupUser);

        IEnumerable<GroupUser> GetForGroup(Guid groupId);

        IEnumerable<GroupUser> GetForUser(Guid userId);
    }
}