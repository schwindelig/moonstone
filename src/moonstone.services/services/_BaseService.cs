using moonstone.core.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class BaseService
    {
        protected RepositoryHub Repositories { get; set; }

        public BaseService(RepositoryHub repoHub)
        {
            this.Repositories = repoHub;
        }
    }
}