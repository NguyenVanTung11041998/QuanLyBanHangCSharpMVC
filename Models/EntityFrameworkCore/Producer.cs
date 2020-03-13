namespace Models.EntityFrameworkCore
{
    public class Producer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Addess { get; set; }
        public string Phone { get; set; }
        public ProducerStatus ProducerStatus { get; set; }
    }

    public enum ProducerStatus
    {
        Deactive = 0,
        Active = 1
    }
}
