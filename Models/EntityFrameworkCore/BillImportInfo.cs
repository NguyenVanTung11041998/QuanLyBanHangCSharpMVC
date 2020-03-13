using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class BillImportInfo
    {
        public long Id { get; set; }
        [ForeignKey(nameof(Product))]
        public long ProductId { get; set; }
        [ForeignKey(nameof(BillImport))]
        public long BillImportId { get; set; }
        public int NumberImport { get; set; }
        public long Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual BillImport BillImport { get; set; }
    }
}
