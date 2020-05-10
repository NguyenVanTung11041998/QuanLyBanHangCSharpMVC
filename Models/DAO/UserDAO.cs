using Abp.Application.Services.Dto;
using Models.DTO;
using Models.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserDAO : BaseDAO
    {
        public async Task<bool> LoginAsync(string email, string password)
        {
            return await DBContext.Accounts.AnyAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<long> CreateUserAsync(User user)
        {
            DBContext.Users.Add(user);
            await DBContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> EditAsync(AccountUserDto accountUser)
        {
            var user = await DBContext.Users.FirstOrDefaultAsync(x => x.Id == accountUser.Id);
            user.Name = accountUser.Name;
            user.Phone = accountUser.Phone;
            user.Address = accountUser.Address;
            if (!string.IsNullOrWhiteSpace(accountUser.Password))
            {
                var account = await DBContext.Accounts.FirstOrDefaultAsync(x => x.Email == accountUser.Email);
                account.Password = accountUser.Password;
            }
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await DBContext.Users.ToListAsync();
        }

        public PagedResultDto<Account> GetAllUsersByKeyWord(string search, int currentPage, int pageSize)
        {
            IQueryable<Account> accounts = DBContext.Accounts.OrderByDescending(x => x.Id);
            if (!string.IsNullOrWhiteSpace(search))
                accounts = accounts.Where(x => x.User.Name.Contains(search.Trim()));
            int totalCount = accounts.Count();
            accounts = accounts.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<Account>(totalCount, accounts.ToList());
        }

        public async Task<UserInfo> GetUserInfoById(long userId)
        {
            return await DBContext.Accounts.Where(x => x.UserId == userId)
                                           .Select(x => new UserInfo
                                           {
                                               Id = x.Id,
                                               Address = x.User.Address,
                                               Email = x.Email,
                                               Name = x.User.Name,
                                               Phone = x.User.Phone,
                                               AccountStatus = x.AccountStatus
                                           }).FirstOrDefaultAsync();
        }
    }
}
