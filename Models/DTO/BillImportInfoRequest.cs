namespace Models.DTO
{
    public class BillImportInfoRequest
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long Price { get; set; }
        public int NumberImport { get; set; }
    }
}
