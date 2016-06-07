using moonstone.core.models;
using moonstone.sql.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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