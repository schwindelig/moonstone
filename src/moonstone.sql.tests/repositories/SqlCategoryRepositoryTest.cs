using FluentAssertions;
using moonstone.core.repositories;
using moonstone.core.services;
using moonstone.sql.repositories;
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
    public class SqlCategoryRepositoryTest
    {
        public ICategoryRepository CategoryRepository { get { return this.Repositories.CategoryRepository; } }
        public RepositoryHub Repositories { get; set; }

        [SetUp]
        public void _SetUp()
        {
            var ctx = TestProvider.GetSqlContext();
            this.Repositories = TestProvider.GetRepositoryHub(ctx);
        }

        [Test]
        public void Create_CanCreateGroup()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);

            var category = TestProvider.GetNewCategory(creator.Id, group.Id);
            category.Id = this.CategoryRepository.Create(category);

            var created = this.CategoryRepository.GetById(category.Id);

            category.Id.Should().NotBe(default(Guid));
            created.Should().NotBeNull();
            created.ShouldBeEquivalentTo(category);
        }

        [Test]
        public void GetById_CanFindGroup()
        {
            var creator = TestProvider.CreateNewUser(this.Repositories.UserRepository);
            var group = TestProvider.CreateNewGroup(this.Repositories.GroupRepository, creator.Id);

            var category = TestProvider.CreateNewCategory(this.CategoryRepository, creator.Id, group.Id);

            var res = this.CategoryRepository.GetById(category.Id);
            res.Should().NotBeNull();
            res.ShouldBeEquivalentTo(category);
        }
    }
}