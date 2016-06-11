using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.models
{
    public class Group
    {
        public DateTime CreateDateUtc { get; set; }
        public Guid CreateUserId { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}