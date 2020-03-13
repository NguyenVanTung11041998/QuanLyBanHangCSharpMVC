namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseQuanLyBanHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillImportInfoes", "NumberImport", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillImportInfoes", "NumberImport");
        }
    }
}
