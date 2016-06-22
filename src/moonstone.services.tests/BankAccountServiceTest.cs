using FluentAssertions;
using moonstone.core.exceptions.serviceExceptions;
using moonstone.core.repositories;
using moonstone.core.services;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services.tests
{
    [TestFixture]
    public class BankAccountServiceTest
    {
        public IBankAccountService BankAccountService
        {
            get
            {
                return this.Services.BankAccountService;
            }
        }

        public RepositoryHub Repositories { get; set; }
        public ServiceHub Services { get; set; }

        [SetUp]
        public void _SetUp()
        {
            var ctx = TestProvider.GetSqlContext();
            this.Repositories = TestProvider.GetRepositoryHub(ctx);
            this.Services = TestProvider.GetServiceHub(this.Repositories);
        }

        [Test]
        public void CreateBankAccount_CanCreateBankAccount()
        {
            var creator = this.Services.UserService.CreateUser(TestProvider.GetNewUser());
            var group = this.Services.GroupService.CreateGroup(TestProvider.GetNewGroup(creator.Id));

            var bankAccount = TestProvider.GetNewBankAccount(creator.Id, group.Id);

            var res = this.BankAccountService.CreateBankAccount(bankAccount);
            bankAccount.Id = res.Id;

            res.Should().NotBeNull();
            res.ShouldBeEquivalentTo(bankAccount);
        }

        [Test]
        public void CreateBankAccount_Throws_IfUserIsNotInGroup()
        {
            var groupCreator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var bankAccountCreator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, groupCreator.Id);

            var bankAccount = TestProvider.GetNewBankAccount(bankAccountCreator.Id, group.Id);

            Assert.Throws<UserNotInGroupException>(
                () => this.BankAccountService.CreateBankAccount(bankAccount));
        }

        [Test]
        public void GetBankAccountById_CanFindById()
        {
            var creator = this.Services.UserService.CreateUser(TestProvider.GetNewUser());
            var group = this.Services.GroupService.CreateGroup(TestProvider.GetNewGroup(creator.Id));

            var bankAccount = this.BankAccountService.CreateBankAccount(TestProvider.GetNewBankAccount(creator.Id, group.Id));

            var res = this.BankAccountService.GetBankAccountById(bankAccount.Id);

            res.Should().NotBeNull();
            res.ShouldBeEquivalentTo(bankAccount);
        }
    }
}