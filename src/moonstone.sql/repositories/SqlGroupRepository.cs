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
    public class SqlGroupRepository : SqlBaseRepository, IGroupRepository
    {
        public SqlGroupRepository(SqlContext context)
            : base(context)
        {
        }

        public Guid Create(Group group)
        {
            try
            {
                return this.Context.RunCommand<Guid>(
                    command: this.Context.InsertCommand<Group>(),
                    param: group,
                    mode: CommandMode.Write).Single();
            }
            catch (Exception e)
            {
                throw new CreateGroupException(
                    $"Failed to create group '{group.Name}'.", e);
            }
        }

        public Group GetById(Guid id)
        {
            try
            {
                return this.Context.RunCommand<Group>(
                    command: this.Context.SelectCommand<Group>("id = @Id"),
                    param: new { Id = id },
                    mode: CommandMode.Read).Single();
            }
            catch (Exception e)
            {
                throw new QueryGroupsException(
                    $"Failed to find group with id {id}.", e);
            }
        }
    }
}