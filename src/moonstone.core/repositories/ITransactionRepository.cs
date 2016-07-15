using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.repositories
{
    public interface ITransactionRepository
    {
        Guid Create(Transaction transaction);

        Transaction GetById(Guid id);
    }
}