using moonstone.core.exceptions.serviceExceptions;
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
    public class BankAccountService : BaseService, IBankAccountService
    {
        protected IGroupService GroupService { get; set; }

        public BankAccountService(RepositoryHub repoHub, IGroupService groupService)
            : base(repoHub)
        {
            this.GroupService = groupService;
        }

        public BankAccount CreateBankAccount(BankAccount bankAccount)
        {
            if (this.GroupService.IsUserInGroup(bankAccount.CreateUserId, bankAccount.GroupId))
            {
                return this.GetBankAccountById(
                this.Repositories.BankAccountRepository.Create(bankAccount));
            }
            else
            {
                throw new UserNotInGroupException(
                    $"Bank account will not be created, since user {bankAccount.CreateUserId} is not in group {bankAccount.GroupId}");
            }
        }

        public BankAccount GetBankAccountById(Guid id)
        {
            return this.Repositories.BankAccountRepository.GetById(id);
        }
    }
}