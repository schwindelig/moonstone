using moonstone.core.models;
using moonstone.sql.context;

namespace moonstone.sql.configs
{
    public abstract class ModelDescriptions
    {
        public static SqlModelDescription<Category> Category()
        {
            return SqlModelDescription<Category>
                .Auto("core", "categories");
        }

        public static SqlModelDescription<Group> Group()
        {
            return SqlModelDescription<Group>
                .Auto("core", "groups");
        }

        public static SqlModelDescription<GroupUser> GroupUser()
        {
            return SqlModelDescription<GroupUser>
                .Auto("core", "groupUsers")
                .WithPrimaryKey(p => p.GroupId)
                .WithPrimaryKey(p => p.UserId);
        }

        public static SqlModelDescription<User> User()
        {
            return SqlModelDescription<User>
                .Auto("core", "users")
                .WithReadOnly(u => u.UserName);
        }
    }
}