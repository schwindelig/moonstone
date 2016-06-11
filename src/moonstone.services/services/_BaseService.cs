using moonstone.core.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services.services
{
    public class BaseService
    {
        protected RepositoryHub RepoHub { get; set; }

        public BaseService(RepositoryHub repoHub)
        {
            this.RepoHub = repoHub;
        }
    }
}