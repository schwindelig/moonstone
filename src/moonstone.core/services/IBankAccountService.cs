using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface IBankAccountService
    {
        BankAccount CreateBankAccount(BankAccount bankAccount);

        BankAccount GetBankAccountById(Guid id);

        IEnumerable<BankAccount> GetBankAccountsForGroup(Guid groupId);
    }
}