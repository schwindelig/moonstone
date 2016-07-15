using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(Transaction transaction);

        Transaction GetTransactionById(Guid id);
    }
}