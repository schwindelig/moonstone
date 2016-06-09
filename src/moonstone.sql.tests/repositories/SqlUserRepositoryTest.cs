using FluentAssertions;
using moonstone.core.models;
using moonstone.sql.configs;
using moonstone.sql.context;
using moonstone.sql.repositories;
using NUnit.Framework;
using System;

namespace moonstone.sql.tests.repositories
{
    [TestFixture]
    public class SqlUserRepositoryTest
    {
        protected SqlContext Context { get; set; }
        protected SqlUserRepository UserRepository { get; set; }

        [SetUp]
        public void _Setup()
        {
            this.Context = new SqlContext("moonstone_dev_tests", ".");
            this.UserRepository = new SqlUserRepository(this.Context);
            this.Context.RegisterModelDescription(ModelDescriptions.User());
        }

        [Test]
        public void Create_Can_Insert_New_User()
        {
            var user = GetNewUser();
            bool doesNotExistBeforeCreation = false;
            bool existsAfterCreation = false;

            doesNotExistBeforeCreation = this.UserRepository.GetByEmail(user.Email) == null;
            this.UserRepository.Create(user);
            existsAfterCreation = this.UserRepository.GetByEmail(user.Email) != null;

            Assert.IsTrue(doesNotExistBeforeCreation);
            Assert.IsTrue(existsAfterCreation);
        }

        [Test]
        public void Create_Returns_Id()
        {
            var user = GetNewUser();

            var id = this.UserRepository.Create(user);

            id.Should().NotBe(default(Guid));
        }

        [Test]
        public void Delete_Can_Delete_User()
        {
            var user = GetNewUser();
            this.UserRepository.Create(user);

            bool userExistsBeforeDelete = false;
            bool userExistsAfterDelete = false;

            user = this.UserRepository.GetByEmail(user.Email);
            userExistsBeforeDelete = user != null;

            this.UserRepository.Delete(user);

            userExistsAfterDelete = this.UserRepository.GetByEmail(user.Email) != null;

            Assert.IsTrue(userExistsBeforeDelete);
            Assert.IsFalse(userExistsAfterDelete);
        }

        [Test]
        public void GetByEmail_Can_Find_User()
        {
            var user = GetNewUser();

            this.UserRepository.Create(user);

            var foundUser = this.UserRepository.GetByEmail(user.Email);

            foundUser.Should().NotBeNull();
            foundUser.Email.ShouldBeEquivalentTo(user.Email);
        }

        [Test]
        public void GetById_Can_Find_User()
        {
            var user = GetNewUser();

            var id = this.UserRepository.Create(user);

            var foundUser = this.UserRepository.GetById(id);

            foundUser.Should().NotBeNull();
            foundUser.Email.ShouldBeEquivalentTo(user.Email);
        }

        [Test]
        public void Update_Can_Update_User()
        {
            var user = GetNewUser();
            Guid userId = this.UserRepository.Create(user);
            user = this.UserRepository.GetByEmail(user.Email);

            user.Email = $"edited_{GetNewUser().Email}";

            this.UserRepository.Update(user);

            var updatedUser = this.UserRepository.GetById(user.Id);
            updatedUser.Email.ShouldBeEquivalentTo(user.Email);
            updatedUser.Id.ShouldBeEquivalentTo(user.Id);
        }

        private static User GetNewUser()
        {
            var user = new User
            {
                Email = $"{Guid.NewGuid()}@schwindelig.ch",
                PasswordHash = "h4$h"
            };

            return user;
        }
    }
}