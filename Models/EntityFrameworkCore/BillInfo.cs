using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class BillInfo
    {
        public long Id { get; set; }
        [ForeignKey(nameof(Bill))]
        public long BillId { get; set; }
        [ForeignKey(nameof(Product))]
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Bill Bill { get; set; }
        public long Price { get; set; }
        public long QuantityPurchased { get; set; }
    }
}
