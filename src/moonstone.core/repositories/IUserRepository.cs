using moonstone.core.models;
using System;

namespace moonstone.core.repositories
{
    public interface IUserRepository
    {
        Guid Create(User user);

        void Delete(User user);

        User GetByEmail(string email);

        User GetById(Guid id);

        void Update(User user);
    }
}