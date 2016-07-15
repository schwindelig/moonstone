﻿using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.repositories
{
    public interface IBankAccountRepository
    {
        Guid Create(BankAccount bankAccount);

        IEnumerable<BankAccount> GetBankAccountsForGroup(Guid groupId);

        BankAccount GetById(Guid id);
    }
}