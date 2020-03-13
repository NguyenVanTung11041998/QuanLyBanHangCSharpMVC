namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBQuanLyBanHang1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        AccountId = c.Long(nullable: false),
                        Price = c.Long(nullable: false),
                        QuantityPurchased = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Cards", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Cards", new[] { "AccountId" });
            DropIndex("dbo.Cards", new[] { "ProductId" });
            DropTable("dbo.Cards");
        }
    }
}
