using moonstone.core.exceptions;
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
    public class CategoryService : BaseService, ICategoryService
    {
        protected IGroupService GroupService { get; set; }

        public CategoryService(RepositoryHub repoHub, IGroupService groupService) : base(repoHub)
        {
            this.GroupService = groupService;
        }

        public Category CreateCategory(Category category)
        {
            if (this.GroupService.IsUserInGroup(category.CreateUserId, category.GroupId))
            {
                return this.GetCategoryById(
                this.Repositories.CategoryRepository.Create(category));
            }
            else
            {
                throw new CreateCategoryException(
                    $"Category {category.Name} will not be created, since user with id {category.CreateUserId} is not in group with id {category.GroupId}.");
            }
        }

        public Category GetCategoryById(Guid id)
        {
            return this.Repositories.CategoryRepository.GetById(id);
        }
    }
}