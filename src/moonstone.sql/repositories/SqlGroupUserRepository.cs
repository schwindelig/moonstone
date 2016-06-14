using moonstone.core.exceptions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.sql.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.repositories
{
    public class SqlGroupUserRepository : SqlBaseRepository, IGroupUserRepository
    {
        public SqlGroupUserRepository(SqlContext context)
            : base(context)
        {
        }

        public void Create(GroupUser groupUser)
        {
            try
            {
                this.Context.RunCommand(
                    command: this.Context.InsertCommand<GroupUser>(),
                    param: groupUser,
                    mode: CommandMode.Write);
            }
            catch (Exception e)
            {
                throw new CreateGroupUserException(
                    $"Failed to create group user.", e);
            }
        }

        public IEnumerable<GroupUser> GetForGroup(Guid groupId)
        {
            try
            {
                return this.Context.RunCommand<GroupUser>(
                    command: this.Context.SelectCommand<GroupUser>("groupId = @GroupId"),
                    param: new { GroupId = groupId },
                    mode: CommandMode.Read);
            }
            catch (Exception e)
            {
                throw new QueryGroupUsersException(
                    $"Failed to users for group with id {groupId}", e);
            }
        }

        public IEnumerable<GroupUser> GetForUser(Guid userId)
        {
            try
            {
                return this.Context.RunCommand<GroupUser>(
                    command: this.Context.SelectCommand<GroupUser>("userId = @UserId"),
                    param: new { UserId = userId },
                    mode: CommandMode.Read);
            }
            catch (Exception e)
            {
                throw new QueryGroupUsersException(
                    $"Failed to groups for user with id {userId}", e);
            }
        }
    }
}