using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.repositories
{
    public class RepositoryContainer
    {
        public IUserRepository UserRepository { get; set; }

        public RepositoryContainer(
            IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }
    }
}