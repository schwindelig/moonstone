using Microsoft.AspNet.Identity;
using moonstone.core.models;
using moonstone.core.repositories;
using System;
using System.Threading.Tasks;

namespace moonstone.authentication.stores
{
    public class UserStore :
        IUserStore<User, Guid>,
        IUserPasswordStore<User, Guid>,
        IUserLockoutStore<User, Guid>
    {
        public IUserRepository UserRepository { get; set; }

        public UserStore(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        public Task CreateAsync(User user)
        {
            try
            {
                this.UserRepository.Create(user);

                return Task.FromResult(0);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

        public Task DeleteAsync(User user)
        {
            try
            {
                this.UserRepository.Delete(user);

                return Task.FromResult(0);
            }
            catch (Exception e)
            {
                return Task.FromException<User>(e);
            }
        }

        public void Dispose()
        {
            // todo: implement dispose
        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            try
            {
                return Task.FromResult(this.UserRepository.GetById(userId));
            }
            catch (Exception e)
            {
                return Task.FromException<User>(e);
            }
        }

        public Task<User> FindByNameAsync(string userName)
        {
            try
            {
                return Task.FromResult(this.UserRepository.GetByEmail(userName));
            }
            catch (Exception e)
            {
                return Task.FromException<User>(e);
            }
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task UpdateAsync(User user)
        {
            try
            {
                this.UserRepository.Update(user);

                return Task.FromResult(0);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
                throw;
            }
        }
    }
}