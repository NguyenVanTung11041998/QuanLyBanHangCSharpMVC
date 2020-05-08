using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFrameworkCore
{
    public class Account
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public AccountRole AccountRole { get; set; }
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }

    public enum AccountStatus
    {
        Deactive = 0,
        Active = 1
    }

    public enum AccountRole
    {
        User = 0,
        Admin = 1,
        Manager = 2
    }
}
