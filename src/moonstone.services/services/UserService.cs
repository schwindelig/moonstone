using moonstone.core.i18n;
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
    public class UserService : BaseService, IUserService
    {
        public UserService(RepositoryHub repoHub)
            : base(repoHub)
        {
        }

        public User CreateUser(User user)
        {
            var userId = this.Repositories.UserRepository.Create(user);
            return this.Repositories.UserRepository.GetById(userId);
        }

        public User GetUserById(Guid userId)
        {
            return this.Repositories.UserRepository.GetById(userId);
        }

        public void SetCulture(Guid userId, string culture)
        {
            var user = this.GetUserById(userId);
            user.Culture = culture;
            this.UpdateUser(user);
        }

        protected void UpdateUser(User user)
        {
            this.Repositories.UserRepository.Update(user);
        }
    }
}