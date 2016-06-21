using moonstone.core.exceptions;
using moonstone.core.exceptions.repositoryExceptions;
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
    public class SqlCategoryRepository : SqlBaseRepository, ICategoryRepository
    {
        public SqlCategoryRepository(SqlContext context) : base(context)
        {
        }

        public Guid Create(Category category)
        {
            try
            {
                return this.Context.RunCommand<Guid>(
                    command: this.Context.InsertCommand<Category>(),
                    param: category,
                    mode: CommandMode.Write).Single();
            }
            catch (Exception e)
            {
                throw new CreateCategoryException(
                    $"Failed to create category.", e);
            }
        }

        public Category GetById(Guid id)
        {
            try
            {
                return this.Context.RunCommand<Category>(
                    command: this.Context.SelectCommand<Category>("id = @Id"),
                    param: new { Id = id },
                    mode: CommandMode.Read).Single();
            }
            catch (Exception e)
            {
                throw new QueryCategoriesException(
                    $"Failed to get category with id {id}", e);
            }
        }
    }
}