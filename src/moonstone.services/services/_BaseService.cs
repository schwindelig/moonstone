using moonstone.core.repositories;
using moonstone.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class BaseService
    {
        private RepositoryHub repoHub;

        protected RepositoryHub Repositories { get; set; }

        protected ServiceHub Services { get; set; }

        public BaseService(RepositoryHub repoHub)
        {
            this.Repositories = repoHub;
        }
    }
}