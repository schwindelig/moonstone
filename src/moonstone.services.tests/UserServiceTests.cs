﻿using FluentAssertions;
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
    public class UserServiceTests
    {
        protected IGroupService GroupService { get; set; }
        protected RepositoryHub Repos { get; set; }
        protected IUserService UserService { get; set; }

        [SetUp]
        public void _SetUp()
        {
            var context = TestProvider.GetSqlContext();
            var repoHub = TestProvider.GetRepositoryHub(context);
            var serviceHub = TestProvider.GetServiceHub(repoHub);
            this.UserService = serviceHub.UserService;
            this.GroupService = serviceHub.GroupService;
            this.Repos = repoHub;
        }

        [Test]
        public void CreateUser_CanCreateUser()
        {
            var user = TestProvider.GetNewUser();

            user = this.UserService.CreateUser(user);

            var result = this.UserService.GetUserById(user.Id);

            result.Id.ShouldBeEquivalentTo(user.Id);
        }

        [Test]
        public void GetUserById_CanFindUser()
        {
            var user = this.UserService.CreateUser(TestProvider.GetNewUser());

            var res = this.UserService.GetUserById(user.Id);

            Assert.IsNotNull(res);
            res.Id.ShouldBeEquivalentTo(user.Id);
            res.Email.ShouldBeEquivalentTo(user.Email);
        }

        [Test]
        public void SetCulture_CanSetCulture()
        {
            string newLanguage = "en-US";
            TestProvider.NEW_USER_DEFAULT_LANGUAGE.Should().NotBeSameAs(newLanguage);

            var user = TestProvider.GetNewUser();
            user = this.UserService.CreateUser(user);

            user.Culture.ShouldBeEquivalentTo(TestProvider.NEW_USER_DEFAULT_LANGUAGE);

            this.UserService.SetCulture(user.Id, newLanguage);

            user = this.UserService.GetUserById(user.Id);
            user.Culture.ShouldBeEquivalentTo(newLanguage);
        }

        [Test]
        public void SetCurrentGroup_CanSetCurrentGroup()
        {
            var user = TestProvider.GetNewUser();
            user = this.UserService.CreateUser(user);

            user.CurrentGroupId.Should().NotHaveValue();

            var group = this.GroupService.CreateGroup(TestProvider.GetNewGroup(user.Id));

            this.UserService.SetCurrentGroup(user.Id, group.Id);
            user = this.UserService.GetUserById(user.Id);
            user.CurrentGroupId.ShouldBeEquivalentTo(group.Id);
        }
    }
}