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
    public class SqlTransactionRepository : SqlBaseRepository, ITransactionRepository
    {
        public SqlTransactionRepository(SqlContext context) : base(context)
        {
        }

        public Guid Create(Transaction transaction)
        {
            try
            {
                return this.Context.RunCommand<Guid>(
                    command: this.Context.InsertCommand<Transaction>(),
                    param: transaction,
                    mode: CommandMode.Write).Single();
            }
            catch (Exception e)
            {
                throw new CreateTransactionException(
                    $"Failed to create transaction", e);
            }
        }

        public Transaction GetById(Guid id)
        {
            try
            {
                return this.Context.RunCommand<Transaction>(
                    command: this.Context.SelectCommand<Transaction>("id = @Id"),
                    param: new { Id = id },
                    mode: CommandMode.Read).Single();
            }
            catch (Exception e)
            {
                throw new QueryTransactionsException(
                    $"Failed to get transaction with id {id}", e);
            }
        }
    }
}