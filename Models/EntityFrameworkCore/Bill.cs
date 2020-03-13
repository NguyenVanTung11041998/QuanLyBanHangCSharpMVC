using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class Bill
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        [ForeignKey(nameof(Account))]
        public long AccountId { get; set; }
        public long TotalPrice { get; set; }
        public long VAT { get; set; }
        public string Address { get; set; }
        public BillStatus BillStatus { get; set; }
        public virtual Account Account { get; set; }
    }

    public enum BillStatus
    {
        AwaitingApproval = 0,
        Cancel = 1,
        Confirmed = 2,
        AlreadyPaid = 3
    }
}
