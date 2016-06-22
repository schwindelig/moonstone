using FluentAssertions;
using moonstone.core.repositories;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.sql.tests.repositories
{
    [TestFixture]
    public class SqlBankAccountRepositoryTest
    {
        public IBankAccountRepository BankAccountRepository
        {
            get
            {
                return this.Repositories.BankAccountRepository;
            }
        }

        public RepositoryHub Repositories { get; set; }

        [SetUp]
        public void _SetUp()
        {
            var ctx = TestProvider.GetSqlContext();
            this.Repositories = TestProvider.GetRepositoryHub(ctx);
        }

        [Test]
        public void Create_CanCreateBankAccount()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);

            var bankAccount = TestProvider.GetNewBankAccount(creator.Id, group.Id);
            bankAccount.Id = this.BankAccountRepository.Create(bankAccount);

            var res = this.BankAccountRepository.GetById(bankAccount.Id);

            bankAccount.Id.Should().NotBe(default(Guid));
            res.ShouldBeEquivalentTo(bankAccount);
        }

        [Test]
        public void GetById_CanFindBankAccount()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);

            var bankAccount = TestProvider.GetNewBankAccount(creator.Id, group.Id);
            bankAccount.Id = this.BankAccountRepository.Create(bankAccount);

            var res = this.BankAccountRepository.GetById(bankAccount.Id);

            res.Should().NotBeNull();
            res.ShouldBeEquivalentTo(bankAccount);
        }
    }
}