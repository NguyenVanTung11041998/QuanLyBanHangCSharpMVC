namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbContextQuanLyBanHang1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "VAT", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "VAT");
        }
    }
}
