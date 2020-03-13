namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuanLyBanHangCSharp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillInfoes", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillInfoes", "Address");
        }
    }
}
