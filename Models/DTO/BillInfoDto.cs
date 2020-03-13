namespace Models.DTO
{
    public class BillInfoDto
    {
        public long Id { get; set; }
        public long BillId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long Price { get; set; }
        public long QuantityPurchased { get; set; }
    }
}
