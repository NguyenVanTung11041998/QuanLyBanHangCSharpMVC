namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuanLyBanHangDB : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Products", name: "CategoryProductId", newName: "CategoryId");
            RenameIndex(table: "dbo.Products", name: "IX_CategoryProductId", newName: "IX_CategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Products", name: "IX_CategoryId", newName: "IX_CategoryProductId");
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "CategoryProductId");
        }
    }
}
