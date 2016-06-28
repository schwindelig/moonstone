using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace moonstone.ui.web.Models.ViewModels.Transaction
{
    public class CreateTransactionViewModel
    {
        public decimal Amount { get; set; }
        public Guid CategoryId { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public Guid? DestinationBankAccountId { get; set; }
        public bool IsBooked { get; set; }
        public Guid SourceBankAccountId { get; set; }
        public string Title { get; set; }
        public DateTime ValueDate { get; set; }
    }
}