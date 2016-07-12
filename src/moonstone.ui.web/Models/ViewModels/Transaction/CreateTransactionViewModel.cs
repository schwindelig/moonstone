using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace moonstone.ui.web.Models.ViewModels.Transaction
{
    public class CreateTransactionViewModel
    {
        public decimal Amount { get; set; }

        [UIHint("CategorySelect")]
        public Guid CategoryId { get; set; }

        [UIHint("CurrencySelect")]
        public string Currency { get; set; }

        public string Description { get; set; }

        [UIHint("BankAccountSelect")]
        public Guid? DestinationBankAccountId { get; set; }

        public bool IsBooked { get; set; }

        [UIHint("BankAccountSelect")]
        public Guid SourceBankAccountId { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime ValueDate { get; set; }
    }
}