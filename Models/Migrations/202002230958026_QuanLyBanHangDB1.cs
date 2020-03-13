namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuanLyBanHangDB1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Description");
        }
    }
}
