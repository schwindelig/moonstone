﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface IUserService
    {
        void SetCulture(Guid userId, string culture);
    }
}