using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Models.EntityFrameworkCore
{
    public class QuanLyBanHangCSharpDbContext : DbContext
    {
        public QuanLyBanHangCSharpDbContext() : base("name=QuanLyBanHangCSharpMVC")
        {
            _ = SqlProviderServices.Instance;
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillImport> BillImports { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<BillImportInfo> BillImportInfos { get; set; }
        public virtual DbSet<BillInfo> BillInfos { get; set; }
        public virtual DbSet<Cart> Cards { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
    }
}
