namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBQuanLyBanHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillInfoes", "QuantityPurchased", c => c.Long(nullable: false));
            AddColumn("dbo.Bills", "Address", c => c.String());
            DropColumn("dbo.BillInfoes", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BillInfoes", "Address", c => c.String());
            DropColumn("dbo.Bills", "Address");
            DropColumn("dbo.BillInfoes", "QuantityPurchased");
        }
    }
}
