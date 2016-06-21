using moonstone.core.exceptions;
using moonstone.core.exceptions.repositoryExceptions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.sql.context;
using System;
using System.Linq;

namespace moonstone.sql.repositories
{
    public class SqlUserRepository : SqlBaseRepository, IUserRepository
    {
        public SqlUserRepository(SqlContext context)
            : base(context)
        {
        }

        public Guid Create(User user)
        {
            try
            {
                return this.Context.RunCommand<Guid>(
                    command: this.Context.InsertCommand<User>(), param: user, mode: CommandMode.Write).SingleOrDefault();
            }
            catch (Exception e)
            {
                throw new CreateUserException(
                    $"Failed to create user.", e);
            }
        }

        public void Delete(User user)
        {
            try
            {
                this.Context.RunCommand(
                    command: this.Context.DeleteCommand<User>("id = @Id"),
                    param: user,
                    mode: CommandMode.Write);
            }
            catch (Exception e)
            {
                throw new DeleteUserException(
                    $"Failed to delete user", e);
            }
        }

        public User GetByEmail(string email)
        {
            try
            {
                var command = this.Context.SelectCommand<User>($"email = @email");
                return this.Context.RunCommand<User>(
                    command: command,
                    param: new { email = email },
                    mode: CommandMode.Read).SingleOrDefault();
            }
            catch (Exception e)
            {
                throw new QueryUsersException(
                    $"Failed to get user by email {email}", e);
            }
        }

        public User GetById(Guid id)
        {
            try
            {
                var command = this.Context.SelectCommand<User>($"id = @id");
                return this.Context.RunCommand<User>(
                    command: command,
                    param: new { id = id },
                    mode: CommandMode.Read).SingleOrDefault();
            }
            catch (Exception e)
            {
                throw new QueryUsersException(
                    $"Failed to get user by Id {id}", e);
            }
        }

        public void Update(User user)
        {
            try
            {
                var command = this.Context.UpdateCommand<User>("id = @id");
                this.Context.RunCommand(
                    command: command,
                    param: user,
                    mode: CommandMode.Write);
            }
            catch (Exception e)
            {
                throw new UpdateUserException(
                    $"Failed to update user.", e);
            }
        }
    }
}