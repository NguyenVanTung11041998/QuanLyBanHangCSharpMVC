namespace Models.EntityFrameworkCore
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserSatus UserSatus { get; set; }
    }

    public enum UserSatus
    {
        Deactive = 0,
        Active = 1
    }
}
