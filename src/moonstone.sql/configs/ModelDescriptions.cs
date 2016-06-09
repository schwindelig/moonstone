using moonstone.core.models;
using moonstone.sql.context;

namespace moonstone.sql.configs
{
    public abstract class ModelDescriptions
    {
        public static SqlModelDescription<User> User()
        {
            return SqlModelDescription<User>
                .Auto("core", "users")
                .WithReadOnly(u => u.UserName);
        }
    }
}