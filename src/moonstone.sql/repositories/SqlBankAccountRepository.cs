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
    public class SqlBankAccountRepository : SqlBaseRepository, IBankAccountRepository
    {
        public SqlBankAccountRepository(SqlContext context)
            : base(context)
        {
        }

        public Guid Create(BankAccount bankAccount)
        {
            try
            {
                return this.Context.RunCommand<Guid>(
                    command: this.Context.InsertCommand<BankAccount>(),
                    param: bankAccount,
                    mode: CommandMode.Write).Single();
            }
            catch (Exception e)
            {
                throw new CreateBankAccountException(
                    $"Failed to created bank account.", e);
            }
        }

        public BankAccount GetById(Guid id)
        {
            try
            {
                return this.Context.RunCommand<BankAccount>(
                    command: this.Context.SelectCommand<BankAccount>("id = @Id"),
                    param: new { Id = id },
                    mode: CommandMode.Read).Single();
            }
            catch (Exception e)
            {
                throw new QueryBankAccountException(
                    $"Failed to find bank account with id {id}", e);
            }
        }
    }
}