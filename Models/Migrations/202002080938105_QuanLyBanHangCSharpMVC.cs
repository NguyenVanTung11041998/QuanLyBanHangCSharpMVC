namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuanLyBanHangCSharpMVC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        UserId = c.Long(nullable: false),
                        AccountStatus = c.Int(nullable: false),
                        AccountRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        UserSatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        ProducerId = c.Long(),
                        ProductId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producers", t => t.ProducerId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProducerId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Addess = c.String(),
                        Phone = c.String(),
                        ProducerStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        AmountProduct = c.Long(nullable: false),
                        Price = c.Long(nullable: false),
                        ProducerId = c.Long(nullable: false),
                        CategoryProductId = c.Long(nullable: false),
                        ProductStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryProductId, cascadeDelete: true)
                .ForeignKey("dbo.Producers", t => t.ProducerId, cascadeDelete: true)
                .Index(t => t.ProducerId)
                .Index(t => t.CategoryProductId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillImportInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        BillImportId = c.Long(nullable: false),
                        Price = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BillImports", t => t.BillImportId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BillImportId);
            
            CreateTable(
                "dbo.BillImports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                        TotalPrice = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        BillImportType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BillInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BillId = c.Long(nullable: false),
                        ProductId = c.Long(nullable: false),
                        Price = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BillId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                        UserId = c.Long(nullable: false),
                        TotalPrice = c.Long(nullable: false),
                        BillStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillInfoes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BillInfoes", "BillId", "dbo.Bills");
            DropForeignKey("dbo.Bills", "UserId", "dbo.Users");
            DropForeignKey("dbo.BillImportInfoes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BillImportInfoes", "BillImportId", "dbo.BillImports");
            DropForeignKey("dbo.BillImports", "UserId", "dbo.Users");
            DropForeignKey("dbo.Assets", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.Products", "CategoryProductId", "dbo.Categories");
            DropForeignKey("dbo.Assets", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.Accounts", "UserId", "dbo.Users");
            DropIndex("dbo.Bills", new[] { "UserId" });
            DropIndex("dbo.BillInfoes", new[] { "ProductId" });
            DropIndex("dbo.BillInfoes", new[] { "BillId" });
            DropIndex("dbo.BillImports", new[] { "UserId" });
            DropIndex("dbo.BillImportInfoes", new[] { "BillImportId" });
            DropIndex("dbo.BillImportInfoes", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CategoryProductId" });
            DropIndex("dbo.Products", new[] { "ProducerId" });
            DropIndex("dbo.Assets", new[] { "ProductId" });
            DropIndex("dbo.Assets", new[] { "ProducerId" });
            DropIndex("dbo.Accounts", new[] { "UserId" });
            DropTable("dbo.Bills");
            DropTable("dbo.BillInfoes");
            DropTable("dbo.BillImports");
            DropTable("dbo.BillImportInfoes");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Producers");
            DropTable("dbo.Assets");
            DropTable("dbo.Users");
            DropTable("dbo.Accounts");
        }
    }
}
