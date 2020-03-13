using Models.EntityFrameworkCore;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class AccountDAO
    {
        private QuanLyBanHangCSharpDbContext dbContext = new QuanLyBanHangCSharpDbContext();
        public async Task<Account> LoginAsync(string email, string password)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<Account> GetAccountByIdAsync(long id)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            dbContext.Accounts.Add(account);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePasswordByEmailAsync(string email, string password)
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email);
            account.Password = password;
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeStatusAccount(string email)
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email);
            account.AccountStatus = account.AccountStatus == AccountStatus.Active ? AccountStatus.Deactive : AccountStatus.Active;
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await dbContext.Accounts.AnyAsync(x => x.Email == email);
        }
    }
}
