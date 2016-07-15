using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.models
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CreateUserId { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public Guid? DestinationBankAccountId { get; set; }
        public Guid GroupId { get; set; }
        public Guid Id { get; set; }

        public bool IsBooked
        {
            get
            {
                return this.ValueDateUtc > DateTime.UtcNow;
            }
        }

        public Guid SourceBankAccountId { get; set; }
        public string Title { get; set; }
        public DateTime ValueDateUtc { get; set; }
    }
}