namespace Models.EntityFrameworkCore
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CategoryStatus CategoryStatus { get; set; }
    }

    public enum CategoryStatus
    {
        Deactive = 0,
        Active = 1
    }
}
