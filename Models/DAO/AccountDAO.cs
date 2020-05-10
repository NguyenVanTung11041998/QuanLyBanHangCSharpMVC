using Models.EntityFrameworkCore;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class AccountDAO : BaseDAO
    {
        public async Task<Account> LoginAsync(string email, string password, bool isAdminOrManager = true)
        {
            if (isAdminOrManager)
                return await DBContext.Accounts.FirstOrDefaultAsync(x => x.Email == email && x.Password == password && x.AccountStatus == AccountStatus.Active && (x.AccountRole == AccountRole.Admin || x.AccountRole == AccountRole.Manager));
            return await DBContext.Accounts.FirstOrDefaultAsync(x => x.Email == email && x.Password == password && x.AccountStatus == AccountStatus.Active && x.AccountRole == AccountRole.User);
        }

        public async Task<Account> GetAccountByIdAsync(long id)
        {
            return await DBContext.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            DBContext.Accounts.Add(account);
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePasswordByEmailAsync(string email, string password)
        {
            var account = await DBContext.Accounts.FirstOrDefaultAsync(x => x.Email == email);
            account.Password = password;
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeStatusAccount(string email)
        {
            var account = await DBContext.Accounts.FirstOrDefaultAsync(x => x.Email == email);
            account.AccountStatus = account.AccountStatus == AccountStatus.Active ? AccountStatus.Deactive : AccountStatus.Active;
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await DBContext.Accounts.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckPhoneExistAsync(string phone)
        {
            return await DBContext.Users.AnyAsync(x => x.Phone == phone);
        }
    }
}
