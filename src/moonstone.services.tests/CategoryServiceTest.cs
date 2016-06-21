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
    public class CategoryServiceTest
    {
        public ICategoryService CategoryService
        {
            get
            {
                return this.Services.CategoryService;
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
        public void CreateGroup_CanCreateGroup()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);
            this.Services.GroupService.AddUserToGroup(creator.Id, group.Id);

            var category = TestProvider.GetNewCategory(creator.Id, group.Id);

            category = this.CategoryService.CreateCategory(category);

            category.Should().NotBeNull();
        }

        [Test]
        public void CreateGroup_Throws_IfUserIsNotInGroup()
        {
            var groupCreator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var categoryCreator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, groupCreator.Id);
            this.Services.GroupService.AddUserToGroup(groupCreator.Id, group.Id);

            Assert.Throws<CreateCategoryException>(() =>
                this.CategoryService.CreateCategory(TestProvider.GetNewCategory(categoryCreator.Id, group.Id)));
        }

        [Test]
        public void GetCategoryById_CanFindCategory()
        {
            var creator = this.Services.UserService.CreateUser(TestProvider.GetNewUser());
            var group = this.Services.GroupService.CreateGroup(TestProvider.GetNewGroup(creator.Id));

            var category = this.CategoryService.CreateCategory(TestProvider.GetNewCategory(creator.Id, group.Id));

            var res = this.CategoryService.GetCategoryById(category.Id);

            res.Should().NotBeNull();
            res.ShouldBeEquivalentTo(category);
        }
    }
}