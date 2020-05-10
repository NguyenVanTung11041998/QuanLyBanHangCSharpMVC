using Models.EntityFrameworkCore;

namespace Models.DAO
{
    public class BaseDAO
    {
        private QuanLyBanHangCSharpDbContext dbContext;
        protected QuanLyBanHangCSharpDbContext DBContext
        {
            get { if (dbContext == null) dbContext = new QuanLyBanHangCSharpDbContext(); return dbContext; }
            private set { dbContext = value; }
        }
    }
}
