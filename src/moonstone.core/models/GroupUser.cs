using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.models
{
    public class GroupUser
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
    }
}