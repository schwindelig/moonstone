﻿using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface ICategoryService
    {
        Category CreateCategory(Category category);

        Category GetCategoryById(Guid id);
    }
}