namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbContextQuanLyBanHang : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bills", "UserId", "dbo.Users");
            DropIndex("dbo.Bills", new[] { "UserId" });
            AddColumn("dbo.Bills", "AccountId", c => c.Long(nullable: false));
            CreateIndex("dbo.Bills", "AccountId");
            AddForeignKey("dbo.Bills", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
            DropColumn("dbo.Bills", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "UserId", c => c.Long(nullable: false));
            DropForeignKey("dbo.Bills", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Bills", new[] { "AccountId" });
            DropColumn("dbo.Bills", "AccountId");
            CreateIndex("dbo.Bills", "UserId");
            AddForeignKey("dbo.Bills", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
