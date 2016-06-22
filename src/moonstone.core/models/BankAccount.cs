using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.models
{
    public class BankAccount
    {
        public Guid CreateUserId { get; set; }
        public string Description { get; set; }
        public Guid GroupId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}