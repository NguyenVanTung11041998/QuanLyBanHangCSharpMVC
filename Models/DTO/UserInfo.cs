using Models.EntityFrameworkCore;

namespace Models.DTO
{
    public class UserInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
