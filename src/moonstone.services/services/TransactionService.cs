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
    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(RepositoryHub repoHub)
            : base(repoHub)
        {
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            // TODO: Validate all foreign keys (bankAccountId, groupId, categoryId, etc.)

            return this.GetTransactionById(
                this.Repositories.TransactionRepository.Create(transaction));
        }

        public Transaction GetTransactionById(Guid id)
        {
            return this.Repositories.TransactionRepository.GetById(id);
        }
    }
}