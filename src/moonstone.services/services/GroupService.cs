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

        public Group CreateGroup(Group group)
        {
            var groupId = this.Repositories.GroupRepository.Create(group);
            return this.GetGroupById(groupId);
        }

        public Group GetGroupById(Guid groupId)
        {
            return this.Repositories.GroupRepository.GetById(groupId);
        }
    }
}