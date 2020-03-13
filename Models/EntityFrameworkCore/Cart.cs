using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class Cart
    {
        public long Id { get; set; }
        [ForeignKey(nameof(Product))]
        public long ProductId { get; set; }
        [ForeignKey(nameof(Account))]
        public long AccountId { get; set; }
        public long Price { get; set; }
        public long QuantityPurchased { get; set; }
        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }
    }
}
