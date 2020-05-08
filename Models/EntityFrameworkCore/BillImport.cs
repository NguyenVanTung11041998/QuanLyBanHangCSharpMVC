using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class BillImport
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long TotalPrice { get; set; }
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public BillImportType BillImportType { get; set; }
        public virtual ICollection<BillImportInfo> BillImportInfos { get; set; }
    }

    public enum BillImportType
    {
        AwaitingApproval = 0,
        Cancel = 1,
        Confirmed = 2
    }
}
